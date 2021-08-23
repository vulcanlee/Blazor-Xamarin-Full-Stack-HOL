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

    public class FlowHistoryService : IFlowHistoryService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<FlowHistoryService> Logger { get; }
        #endregion

        #region 建構式
        public FlowHistoryService(BackendDBContext context, IMapper mapper,
            ILogger<FlowHistoryService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<FlowHistoryAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<FlowHistoryAdapterModel> data = new();
            DataRequestResult<FlowHistoryAdapterModel> result = new();
            var DataSource = context.FlowHistory
                .AsNoTracking()
                .Include(x => x.MyUser)
                .AsQueryable();

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Summary.Contains(dataRequest.Search) ||
                x.Comment.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)FlowHistorySortEnum.CreateDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Updatetime);
                        break;
                    case (int)FlowHistorySortEnum.CreateDateAscending:
                        DataSource = DataSource.OrderBy(x => x.Updatetime);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<FlowHistory>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowHistoryAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowHistoryAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<FlowHistoryAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<FlowHistoryAdapterModel> data = new();
            DataRequestResult<FlowHistoryAdapterModel> result = new();
            var DataSource = context.FlowHistory
                .AsNoTracking()
                .Include(x => x.MyUser)
                .Where(x => x.FlowMasterId == id);

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Summary.Contains(dataRequest.Search)||
                x.Comment.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)FlowHistorySortEnum.CreateDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Updatetime);
                        break;
                    case (int)FlowHistorySortEnum.CreateDateAscending:
                        DataSource = DataSource.OrderBy(x => x.Updatetime);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<FlowHistory>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowHistoryAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowHistoryAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<FlowHistoryAdapterModel> GetAsync(int id)
        {
            FlowHistory item = await context.FlowHistory
                .AsNoTracking()
                .Include(x => x.MyUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            FlowHistoryAdapterModel result = Mapper.Map<FlowHistoryAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(FlowHistoryAdapterModel paraObject)
        {
            try
            {
                FlowHistory itemParameter = Mapper.Map<FlowHistory>(paraObject);
                CleanTrackingHelper.Clean<FlowHistory>(context);
                await context.FlowHistory
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<FlowHistory>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(FlowHistoryAdapterModel paraObject)
        {
            try
            {
                FlowHistory itemData = Mapper.Map<FlowHistory>(paraObject);
                CleanTrackingHelper.Clean<FlowHistory>(context);
                FlowHistory item = await context.FlowHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowHistory>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowHistory>(context);
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
                CleanTrackingHelper.Clean<FlowHistory>(context);
                FlowHistory item = await context.FlowHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowHistory>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowHistory>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(FlowHistoryAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowHistory>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            var item = await context.FlowHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FlowMasterId == paraObject.FlowMasterId &&
                x.MyUserId == paraObject.MyUserId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowHistoryAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowHistory>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }

            var searchItem = await context.FlowHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.FlowHistory
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

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowHistoryAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowHistory>(context);
            var searchItem = await context.FlowHistory
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
        Task OhterDependencyData(FlowHistoryAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            return Task.FromResult(0);
        }
        #endregion

    }
}
