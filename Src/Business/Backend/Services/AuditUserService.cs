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
    using Microsoft.Extensions.Logging;
    using BAL.Factories;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using CommonDomain.Enums;
    using System;

    public class AuditUserService : IAuditUserService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<AuditUserService> Logger { get; }
        #endregion

        #region 建構式
        public AuditUserService(BackendDBContext context, IMapper mapper,
            ILogger<AuditUserService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<AuditUserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<AuditUserAdapterModel> data = new();
            DataRequestResult<AuditUserAdapterModel> result = new();
            var DataSource = context.AuditUser
                .AsNoTracking()
                .Include(x => x.MyUser)
                .AsQueryable();

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)AuditUserSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)AuditUserSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<AuditUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<AuditUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<AuditUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<AuditUserAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<AuditUserAdapterModel> data = new();
            DataRequestResult<AuditUserAdapterModel> result = new();
            var DataSource = context.AuditUser
                .AsNoTracking()
                .Include(x => x.MyUser)
                .Where(x => x.AuditMasterId == id);

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)AuditUserSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)AuditUserSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<AuditUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<AuditUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<AuditUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<AuditUserAdapterModel> GetAsync(int id)
        {
            AuditUser item = await context.AuditUser
                .AsNoTracking()
                .Include(x => x.MyUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            AuditUserAdapterModel result = Mapper.Map<AuditUserAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(AuditUserAdapterModel paraObject)
        {
            try
            {
                AuditUser itemParameter = Mapper.Map<AuditUser>(paraObject);
                CleanTrackingHelper.Clean<AuditUser>(context);
                await context.AuditUser
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<AuditUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(AuditUserAdapterModel paraObject)
        {
            try
            {
                AuditUser itemData = Mapper.Map<AuditUser>(paraObject);
                CleanTrackingHelper.Clean<AuditUser>(context);
                AuditUser item = await context.AuditUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AuditUser>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AuditUser>(context);
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
                CleanTrackingHelper.Clean<AuditUser>(context);
                AuditUser item = await context.AuditUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AuditUser>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AuditUser>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(AuditUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<AuditUser>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            var item = await context.AuditUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AuditMasterId == paraObject.AuditMasterId &&
                x.MyUserId == paraObject.MyUserId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(AuditUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<AuditUser>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }

            var searchItem = await context.AuditUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.AuditUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AuditMasterId == paraObject.AuditMasterId &&
                x.MyUserId == paraObject.MyUserId &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(AuditUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<AuditUser>(context);
            var searchItem = await context.AuditUser
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(AuditUserAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            return Task.FromResult(0);
        }
        #endregion

        #region 啟用或停用的紀錄變更
        public async Task DisableIt(AuditUserAdapterModel paraObject)
        {
            AuditUser itemData = Mapper.Map<AuditUser>(paraObject);
            CleanTrackingHelper.Clean<AuditUser>(context);
            AuditUser item = await context.AuditUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = false;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }

        public async Task EnableIt(AuditUserAdapterModel paraObject)
        {
            AuditUser itemData = Mapper.Map<AuditUser>(paraObject);
            CleanTrackingHelper.Clean<AuditUser>(context);
            AuditUser item = await context.AuditUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = true;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }
        #endregion
    }
}
