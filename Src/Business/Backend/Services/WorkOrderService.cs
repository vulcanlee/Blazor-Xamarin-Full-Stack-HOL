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
    using Backend.Models;

    public class WorkOrderService : IWorkOrderService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<WorkOrderService> Logger { get; }
        #endregion

        #region 建構式
        public WorkOrderService(BackendDBContext context, IMapper mapper,
            ILogger<WorkOrderService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<WorkOrderAdapterModel>> GetAsync(DataRequest dataRequest,
            WorkOrderStatusCondition CurrentWorkOrderStatusCondition = null)
        {
            List<WorkOrderAdapterModel> data = new();
            DataRequestResult<WorkOrderAdapterModel> result = new();
            var DataSource = context.WorkOrder
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Description.Contains(dataRequest.Search) ||
                x.Comment.Contains(dataRequest.Search));
            }
            #endregion

            #region 過濾派工單狀態
            if(CurrentWorkOrderStatusCondition!=null)
            {
                if(CurrentWorkOrderStatusCondition.Id != -1)
                {
                    DataSource = DataSource
                    .Where(x => x.Status== CurrentWorkOrderStatusCondition.Id);
                }
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)WorkOrderSortEnum.CreatedAtDescending:
                        DataSource = DataSource.OrderByDescending(x => x.CreatedAt);
                        break;
                    case (int)WorkOrderSortEnum.CreatedAtAscending:
                        DataSource = DataSource.OrderBy(x => x.CreatedAt);
                        break;
                    case (int)WorkOrderSortEnum.StartDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.StartDate);
                        break;
                    case (int)WorkOrderSortEnum.StartDateAscending:
                        DataSource = DataSource.OrderBy(x => x.StartDate);
                        break;
                    case (int)WorkOrderSortEnum.StatusDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Status);
                        break;
                    case (int)WorkOrderSortEnum.StatusAscending:
                        DataSource = DataSource.OrderBy(x => x.Status);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<WorkOrder>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<WorkOrderAdapterModel> adapterModelObjects =
                Mapper.Map<List<WorkOrderAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<WorkOrderAdapterModel> GetAsync(int id)
        {
            WorkOrder item = await context.WorkOrder
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            WorkOrderAdapterModel result = Mapper.Map<WorkOrderAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(WorkOrderAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<WorkOrder>(context);
                WorkOrder itemParameter = Mapper.Map<WorkOrder>(paraObject);
                CleanTrackingHelper.Clean<WorkOrder>(context);
                await context.WorkOrder
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<WorkOrder>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(WorkOrderAdapterModel paraObject)
        {
            try
            {
                WorkOrder itemData = Mapper.Map<WorkOrder>(paraObject);
                CleanTrackingHelper.Clean<WorkOrder>(context);
                WorkOrder item = await context.WorkOrder
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<WorkOrder>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<WorkOrder>(context);
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
                CleanTrackingHelper.Clean<WorkOrder>(context);
                WorkOrder item = await context.WorkOrder
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<WorkOrder>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<WorkOrder>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(WorkOrderAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkOrderAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<WorkOrder>(context);
            var searchItem = await context.WorkOrder
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkOrderAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<WorkOrder>(context);

                var searchItem = await context.WorkOrder
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (searchItem == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
                }

                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                return VerifyRecordResultFactory.Build(false, "刪除記錄發生例外異常", ex);
            }
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(WorkOrderAdapterModel data)
        {
            return Task.FromResult(0);
        }
        #endregion
    }
}
