using AutoMapper;
using Backend.Models;
using EFCore.BulkExtensions;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using BAL.Helpers;
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
                case "功能表角色":
                    await Import功能表角色匯入Async();
                    break;
                default:
                    break;
            }
            IsLoad = false;
            workbook = null;
            sheet = null;
        }

        #region 匯入資料
        #region 使用者匯入
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
                var Email = sheet[i, 6].Value;

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
                        Status = (啟用 == "TRUE") ? true : false,
                        Salt = Guid.NewGuid().ToString(),
                        ForceLogoutDatetime = DateTime.Now.AddDays(-1),
                        ForceChangePassword = false,
                        ForceChangePasswordDatetime = DateTime.Now.AddDays(42),
                        LoginFailTimes = 0,
                        LoginFailUnlockDatetime = DateTime.Now.AddDays(-1),
                        LastLoginDatetime = DateTime.Now,
                        Email = Email,
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
        #endregion

        #region 功能表角色匯入
        public async Task Import功能表角色匯入Async()
        {
            var total = sheet.Rows.Length;
            ShowStatusHandler?.Invoke($"");

            #region 開始進行 功能表角色
            List<MenuRole> foundMenuRoleInsert = new List<MenuRole>();
            List<MenuRole> needMenuRoleInsert = new List<MenuRole>();
            List<MenuData> needMenuDataInsert = new List<MenuData>();
            List<MenuRole> needUpdate = new List<MenuRole>();

            var menuRoles = await Context.MenuRole
                .Include(x => x.MenuData)
                .ToListAsync();

            CleanTrackingHelper.Clean<MenuRole>(Context);
            CleanTrackingHelper.Clean<MenuData>(Context);
            #region 讀取要匯入的資料，轉成中間紀錄
            List<功能表角色匯入> all功能表角色匯入 = new List<功能表角色匯入>();
            for (int i = 2; i <= total; i++)
            {
                ShowStatusHandler?.Invoke($"匯入 功能表角色 {i} / {total}");

                // 1             2    3    4       5       6     7          8         9
                //角色名稱      名稱  啟用  層級  子功能表 排序值 Icon名稱    路由作業    強制導航
                //開發者角色     首頁 TRUE 0       FALSE   10    mdi-home    /         FALSE
                var item功能表角色匯入 = new 功能表角色匯入();
                item功能表角色匯入.角色名稱 = sheet[i, 1].Value;
                item功能表角色匯入.名稱 = sheet[i, 2].Value;
                item功能表角色匯入.啟用 = sheet[i, 3].Value;
                item功能表角色匯入.層級 = sheet[i, 4].Value;
                item功能表角色匯入.子功能表 = sheet[i, 5].Value;
                item功能表角色匯入.排序值 = sheet[i, 6].Value;
                item功能表角色匯入.Icon名稱 = sheet[i, 7].Value;
                item功能表角色匯入.路由作業 = sheet[i, 8].Value;
                item功能表角色匯入.強制導航 = sheet[i, 9].Value;

                all功能表角色匯入.Add(item功能表角色匯入);
            }
            #endregion

            #region 轉換 all功能表角色匯入 成為 MenuData
            foreach (var item in all功能表角色匯入)
            {
                var searchItem = foundMenuRoleInsert.FirstOrDefault(x => x.Name == item.角色名稱);
                #region 確認與建立 MenuRole
                if (searchItem == null)
                {
                    searchItem = new MenuRole()
                    {
                        Name = item.角色名稱,
                        MenuData = new List<MenuData>(),
                        Remark = "",
                    };
                    foundMenuRoleInsert.Add(searchItem);
                }
                #endregion

                #region 建立 MenuData
                var itemMenuData = new MenuData()
                {
                    CodeName = item.路由作業,
                    Enable = item.啟用 == "TRUE" ? true : false,
                    ForceLoad = item.強制導航 == "TRUE" ? true : false,
                    Icon = item.Icon名稱,
                    IsGroup = item.子功能表 == "TRUE" ? true : false,
                    Level = Convert.ToInt32(item.層級),
                    Name = item.名稱,
                    Sequence = Convert.ToInt32(item.排序值),
                };
                searchItem.MenuData.Add(itemMenuData);
                #endregion
            }
            #endregion

            #region 產生要更新資料庫的紀錄
            CleanTrackingHelper.Clean<MenuRole>(Context);
            CleanTrackingHelper.Clean<MenuData>(Context);
            foreach (var item in foundMenuRoleInsert)
            {
                var searchItem = await Context.MenuRole
                    .FirstOrDefaultAsync(x => x.Name == item.Name);
                if (searchItem == null)
                {
                    await Context.AddAsync(item);
                    await Context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MenuRole>(Context);
                    CleanTrackingHelper.Clean<MenuData>(Context);
                }
                else
                {
                    foreach (var itemData in item.MenuData)
                    {
                        var searchItemMenuData = await Context.MenuData
                            .FirstOrDefaultAsync(x => x.MenuRoleId == searchItem.Id &&
                            x.CodeName == itemData.CodeName);
                        if (searchItemMenuData == null)
                        {
                            itemData.MenuRoleId = searchItem.Id;
                            needMenuDataInsert.Add(itemData);
                        }
                    }
                }
            }
            if (needMenuDataInsert.Count > 0)
            {
                await Context.BulkInsertAsync(needMenuDataInsert);
            }
            #endregion

            CleanTrackingHelper.Clean<MenuRole>(Context);
            CleanTrackingHelper.Clean<MenuData>(Context);
            #endregion

            IsLoad = false;
            workbook = null;
            sheet = null;
            ShowStatusHandler?.Invoke($"");

            await Task.Yield();
        }
        #endregion
        #endregion
    }
}
