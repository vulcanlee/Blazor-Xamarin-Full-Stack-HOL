﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using ShareBusiness.Factories;
    using ShareBusiness.Helpers;
    using ShareDomain.DataModels;
    using ShareDomain.Enums;

    public class MyUserService : IMyUserService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }

        public MyUserService(BackendDBContext context, IMapper mapper,
            IConfiguration configuration)
        {
            this.context = context;
            Mapper = mapper;
            Configuration = configuration;
        }

        public async Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MyUserAdapterModel> data = new List<MyUserAdapterModel>();
            DataRequestResult<MyUserAdapterModel> result = new DataRequestResult<MyUserAdapterModel>();
            var DataSource = context.MyUser
                .Include(x => x.MenuRole)
                .AsNoTracking();

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.Account.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)MyUserSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)MyUserSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)MyUserSortEnum.AccountDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Account);
                        break;
                    case (int)MyUserSortEnum.AccountAscending:
                        DataSource = DataSource.OrderBy(x => x.Account);
                        break;
                    case (int)MyUserSortEnum.DepartmentNameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.DepartmentName);
                        break;
                    case (int)MyUserSortEnum.DepartmentNameAscending:
                        DataSource = DataSource.OrderBy(x => x.DepartmentName);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<MyUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MyUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<MyUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<MyUserAdapterModel> GetAsync(int id)

        {
            MyUser item = await context.MyUser
                .Include(x => x.MenuRole)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                MyUserAdapterModel result = Mapper.Map<MyUserAdapterModel>(item);
                await OhterDependencyData(result);
                return result;
            }
            else
            {
                return new MyUserAdapterModel();
            }
        }

        public async Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject)
        {
            MyUser itemParameter = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            await context.MyUser
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<MyUser>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject)
        {
            MyUser itemData = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<MyUser>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<MyUser>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject)
        {
            var searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject)
        {
            var searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject)
        {
            return Task.FromResult(VerifyRecordResultFactory.Build(true));
        }
        public async Task<(MyUserAdapterModel, string)> CheckUser(string account, string password)
        {
            MyUser user = new MyUser();
            MyUserAdapterModel userAdapterModel = new MyUserAdapterModel();
            if (account == MagicHelper.系統管理員帳號)
            {
                #region 進行系統管理者的帳號、密碼的驗證
                var rawPassword = Configuration["AdministratorPassword"];
                if (password != rawPassword)
                {
                    return (null, ErrorMessageMappingHelper.Instance
                        .GetErrorMessage(ErrorMessageEnum.密碼不正確));
                }
                else
                {
                    #region 開發者帳號也需要在資料庫上有存在
                    user = await context.MyUser
                        .Include(x=>x.MenuRole)
                        .ThenInclude(x=>x.MenuData)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Account == account);

                    if (user == null)
                    {
                        return (null, ErrorMessageMappingHelper.Instance
                            .GetErrorMessage(ErrorMessageEnum.密碼不正確));

                    }
                    #endregion

                    #region 取出 開發者功能表角色 功能清單項目
                    var menuRole = await context.MenuRole
                        .AsNoTracking()
                        .Include(x => x.MenuData)
                        .FirstOrDefaultAsync(x => x.Name == MagicHelper.開發者功能表角色);
                    #endregion
             
                    #region 建立預設管理者帳號的物件
                    user.MenuRoleId = menuRole.Id;
                    user.MenuRole = menuRole;

                    userAdapterModel = Mapper.Map<MyUserAdapterModel>(user);
                    #endregion
                }
                #endregion
            }
            else
            {
                user = await context.MyUser
                    .Include(x => x.MenuRole)
                    .ThenInclude(x => x.MenuData)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Account == account);
     
                if (user == null)
                {
                    return (null, ErrorMessageMappingHelper.Instance
                        .GetErrorMessage(ErrorMessageEnum.使用者帳號不存在));
                }

                var shaPassword =
                    PasswordHelper.GetPasswordSHA(user.Salt, password);

                if (user.Password != shaPassword)
                {
                    return (null, ErrorMessageMappingHelper.Instance
                        .GetErrorMessage(ErrorMessageEnum.密碼不正確));
                }
                userAdapterModel = Mapper.Map<MyUserAdapterModel>(user);
            }
            return (userAdapterModel, "");
        }

        async Task OhterDependencyData(MyUserAdapterModel data)
        {
            if (data.IsManager == true)
            {
                data.IsManagerString = "是";
            }
            else
            {
                data.IsManagerString = "否";
            }

            var user = await context.MyUser
                .FirstOrDefaultAsync(x => x.Id == data.ManagerId);
            if (user != null)
            {
                data.ManagerName = user.Name;
            }

            data.MenuRoleName = data.MenuRole.Name;
        }
    }
}
