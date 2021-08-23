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

    public class FlowUserService : IFlowUserService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<FlowUserService> Logger { get; }
        #endregion

        #region 建構式
        public FlowUserService(BackendDBContext context, IMapper mapper,
            ILogger<FlowUserService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<FlowUserAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<FlowUserAdapterModel> data = new();
            DataRequestResult<FlowUserAdapterModel> result = new();
            var DataSource = context.FlowUser
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
                    case (int)FlowUserSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)FlowUserSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<FlowUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<FlowUserAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<FlowUserAdapterModel> data = new();
            DataRequestResult<FlowUserAdapterModel> result = new();
            var DataSource = context.FlowUser
                .AsNoTracking()
                .Include(x => x.MyUser)
                .Where(x => x.FlowMasterId == id);

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
                    case (int)FlowUserSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)FlowUserSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<FlowUser>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowUserAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowUserAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<FlowUserAdapterModel> GetAsync(int id)
        {
            FlowUser item = await context.FlowUser
                .AsNoTracking()
                .Include(x => x.MyUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            FlowUserAdapterModel result = Mapper.Map<FlowUserAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(FlowUserAdapterModel paraObject)
        {
            try
            {
                FlowUser itemParameter = Mapper.Map<FlowUser>(paraObject);
                CleanTrackingHelper.Clean<FlowUser>(context);
                await context.FlowUser
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<FlowUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(FlowUserAdapterModel paraObject)
        {
            try
            {
                FlowUser itemData = Mapper.Map<FlowUser>(paraObject);
                CleanTrackingHelper.Clean<FlowUser>(context);
                FlowUser item = await context.FlowUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowUser>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowUser>(context);
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
                CleanTrackingHelper.Clean<FlowUser>(context);
                FlowUser item = await context.FlowUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowUser>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowUser>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(FlowUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowUser>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            var item = await context.FlowUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FlowMasterId == paraObject.FlowMasterId &&
                x.MyUserId == paraObject.MyUserId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowUser>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }

            var searchItem = await context.FlowUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.FlowUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FlowMasterId == paraObject.FlowMasterId &&
                x.MyUserId == paraObject.MyUserId &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowUserAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowUser>(context);
            var searchItem = await context.FlowUser
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
        Task OhterDependencyData(FlowUserAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            return Task.FromResult(0);
        }
        #endregion

        #region 啟用或停用的紀錄變更
        public async Task DisableIt(FlowUserAdapterModel paraObject)
        {
            FlowUser itemData = Mapper.Map<FlowUser>(paraObject);
            CleanTrackingHelper.Clean<FlowUser>(context);
            FlowUser item = await context.FlowUser
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

        public async Task EnableIt(FlowUserAdapterModel paraObject)
        {
            FlowUser itemData = Mapper.Map<FlowUser>(paraObject);
            CleanTrackingHelper.Clean<FlowUser>(context);
            FlowUser item = await context.FlowUser
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
