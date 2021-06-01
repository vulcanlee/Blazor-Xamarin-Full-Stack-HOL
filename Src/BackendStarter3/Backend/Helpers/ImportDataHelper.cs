using AutoMapper;
using EFCore.BulkExtensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ShareBusiness.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class ImportDataHelper
    {
        public Action<string> ShowStatusHandler;
        public string CurrentFile { get; set; } = "";
        ExcelEngine excelEngine = new ExcelEngine();
        IWorkbook workbook;
        IWorksheet sheet;
        public bool IsLoad = false;

        public ImportDataHelper(BackendDBContext context, IMapper Mapper)
        {
            Context = context;
            this.Mapper = Mapper;
        }

        public string ImportName { get; set; }
        public BackendDBContext Context { get; }
        public IMapper Mapper { get; }

        /// <summary>
        /// 取得該匯入 Excel 檔案的類型
        /// </summary>
        public async Task GetUploadFileAsync(MemoryStream inputFileName)
        {
            IsLoad = true;
            await Task.Yield();
            try
            {
                MemoryStream filestream = new MemoryStream();
                inputFileName.WriteTo(filestream);
                inputFileName.Seek(0L, SeekOrigin.Begin);
                //Loads or open an existing workbook through Open method of IWorkbooks
                workbook = excelEngine.Excel.Workbooks.Open(inputFileName);
                sheet = workbook.Worksheets[0];
                ImportName = sheet.CodeName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsLoad = false;
            }
        }

        public async Task ImportAsync()
        {
            switch (ImportName)
            {
                case "使用者":
                    await Import使用者匯入Async();
                    break;
                default:
                    break;
            }
            IsLoad = false;
            workbook = null;
            sheet = null;
        }
        public async Task Import使用者匯入Async()
        {
            var total = sheet.Rows.Length;
            ShowStatusHandler?.Invoke($"");

            #region 開始進行 使用者
            List<MyUser> needInsert = new List<MyUser>();
            List<MyUser> needUpdate = new List<MyUser>();

            var menuRoles = await Context.MenuRole
                .Include(x => x.MenuData)
                .ToListAsync();

            CleanTrackingHelper.Clean<MyUser>(Context);
            for (int i = 2; i <= total; i++)
            {
                ShowStatusHandler?.Invoke($"匯入使用者 {i} / {total}");

                //姓名 帳號  密碼 啟用  角色
                //黃○瑋 acc01   123 1   使用者角色

                var 姓名 = sheet[i, 1].Value;
                var 帳號 = sheet[i, 2].Value;
                var 密碼 = sheet[i, 3].Value;
                var 啟用 = sheet[i, 4].Value;
                var 角色 = sheet[i, 5].Value;

                var menuItem = menuRoles.FirstOrDefault(x => x.Name == 角色);
                if (menuItem == null) continue;

                var item = await Context.MyUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Account == 帳號);
                if (item == null)
                {
                    item = new MyUser()
                    {
                        Account = 帳號,
                        Name = 姓名,
                        Status = (啟用 == "1") ? true : false,
                        Password = "",
                        Salt = Guid.NewGuid().ToString(),
                    };
                    item.Password = PasswordHelper.GetPasswordSHA(item.Salt, 密碼);
                    item.MenuRoleId = menuItem.Id;
                    needInsert.Add(item);
                }
                else
                {
                }
            }
            if (needInsert.Any())
            {
                ShowStatusHandler?.Invoke($"正在新增使用者，共 {needInsert.Count}");
                await Context.BulkInsertAsync(needInsert);
            }
            CleanTrackingHelper.Clean<MyUser>(Context);
            #endregion

            IsLoad = false;
            workbook = null;
            sheet = null;
            ShowStatusHandler?.Invoke($"");

            await Task.Yield();
        }
    }
}
