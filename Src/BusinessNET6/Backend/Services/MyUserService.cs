using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Domains.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using BAL.Factories;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using CommonDomain.Enums;
    using System;

    public class MyUserService : IMyUserService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }
        public ILogger<MyUserService> Logger { get; }
        #endregion

        #region 建構式
        public MyUserService(BackendDBContext context, IMapper mapper,
            IConfiguration configuration, ILogger<MyUserService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Configuration = configuration;
            Logger = logger;
        }
        #endregion


        #region CRUD 服務
        public async Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MyUserAdapterModel> data = new();
            DataRequestResult<MyUserAdapterModel> result = new();
            var DataSource = context.MyUser
                .Include(x => x.MenuRole)
                .ThenInclude(x => x.MenuData)
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
                .ThenInclude(x => x.MenuData)
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
                return new MyUserAdapterModel() { Status = false };
            }
        }

        public async Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject)
        {
            try
            {
                MyUser itemParameter = Mapper.Map<MyUser>(paraObject);
                CleanTrackingHelper.Clean<MyUser>(context);
                await context.MyUser
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(ex, "修改記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "修改記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(ex, "刪除記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "刪除記錄發生例外異常", ex);
            }
        }
        #endregion


        #region CRUD 的限制條件檢查
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject)
        {
            if (paraObject.Account.ToLower() == MagicHelper.開發者帳號)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.開發者帳號不可以被新增);
            }

            var searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == paraObject.Account);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            var searchMenuRoleItem = await context.MenuRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.MenuRoleId);
            if (searchMenuRoleItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.沒有指定功能表角色項目);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject)
        {
            //if (paraObject.Account.ToLower() == MagicHelper.開發者帳號)
            //{
            //    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.開發者帳號不可以被修改);
            //}

            CleanTrackingHelper.Clean<MyUser>(context);
            var searchItem = await context.MyUser
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem != null)
            {
                //if (searchItem.Account.ToLower() == MagicHelper.開發者帳號)
                //{
                //    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.開發者帳號不可以被修改);
                //}
            }
            else
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.MyUser
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Account == paraObject.Account &&
               x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject)
        {
            if (paraObject.Account.ToLower() == MagicHelper.開發者帳號)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.開發者帳號不可以被刪除);
            }

            CleanTrackingHelper.Clean<MyUser>(context);
            var searchItem = await context.MyUser
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
            }

            searchItem = await context.MyUser
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem != null)
            {
                if (searchItem.Account.ToLower() == MagicHelper.開發者帳號)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.開發者帳號不可以被刪除);
                }
            }
            else
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        #endregion

        #region 其他服務方法
        public async Task<MyUserAdapterModel> UserByAccount(string account)
        {
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser user = new();
            MyUserAdapterModel userAdapterModel = new();
            user = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Account == account);
            userAdapterModel = Mapper.Map<MyUserAdapterModel>(user);
            CleanTrackingHelper.Clean<MyUser>(context);
            return userAdapterModel;
        }

        public async Task<(MyUserAdapterModel, string)> CheckUser(string account, string password)
        {
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser user = new();
            MyUserAdapterModel userAdapterModel = new();
            if (account == MagicHelper.開發者帳號)
            {
                #region 進行開發者帳號、密碼的驗證
                var GodPasswordSlat = Configuration[AppSettingHelper.GodPasswordSlat];
                var GodrPasswordHash = Configuration[AppSettingHelper.GodrPasswordHash];

                var EncodePassword =
                      PasswordHelper.GetGodPasswordSHA(GodPasswordSlat, password);

                //Logger.LogInformation($"rawPassword:{rawPassword}");
                //Logger.LogInformation($"password:{password}");
                if (EncodePassword != GodrPasswordHash)
                {
                    return (null, ErrorMessageMappingHelper.Instance
                        .GetErrorMessage(ErrorMessageEnum.密碼不正確));
                }
                else
                {
                    #region 開發者帳號也需要在資料庫上有存在
                    user = await context.MyUser
                        .Include(x => x.MenuRole)
                        .ThenInclude(x => x.MenuData)
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
                    user.Status = true;
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
                    var AccountPolicy = await context.AccountPolicy
                        .OrderBy(x => x.Id).FirstOrDefaultAsync();
                    user.LoginFailTimes++;
                    if (user.LoginFailTimes >= AccountPolicy.LoginFailMaxTimes)
                    {
                        user.LoginFailUnlockDatetime = DateTime
                            .Now.AddMinutes(AccountPolicy.LoginFailTimesLockMinutes);
                    }

                    context.MyUser.Update(user);
                    await context.SaveChangesAsync();

                    return (null, ErrorMessageMappingHelper.Instance
                        .GetErrorMessage(ErrorMessageEnum.密碼不正確));
                }

                #region 重新 Reset 計算登入失敗的計數器
                user.LoginFailTimes = 0;
                user.LoginFailUnlockDatetime = DateTime.Now.AddMinutes(-5);
                context.MyUser.Update(user);
                await context.SaveChangesAsync();
                #endregion

                CleanTrackingHelper.Clean<MyUser>(context);
                userAdapterModel = Mapper.Map<MyUserAdapterModel>(user);
            }
            return (userAdapterModel, "");
        }

        async Task OhterDependencyData(MyUserAdapterModel data)
        {
            await Task.Yield();
            data.MenuRoleName = data.MenuRole.Name;
        }
        #endregion

        #region 紀錄啟用或停用
        public async Task DisableIt(MyUserAdapterModel paraObject)
        {
            MyUser itemData = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Status = false;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }

        public async Task EnableIt(MyUserAdapterModel paraObject)
        {
            MyUser itemData = Mapper.Map<MyUser>(paraObject);
            CleanTrackingHelper.Clean<MyUser>(context);
            MyUser item = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Status = true;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }
        #endregion
    }
}
