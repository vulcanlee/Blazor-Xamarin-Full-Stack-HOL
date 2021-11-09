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

    public class SystemLogService : ISystemLogService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<SystemLogService> Logger { get; }
        #endregion

        #region 建構式
        public SystemLogService(BackendDBContext context, IMapper mapper,
            ILogger<SystemLogService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<SystemLogAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<SystemLogAdapterModel> data = new();
            DataRequestResult<SystemLogAdapterModel> result = new();
            var DataSource = context.SystemLog
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Message.Contains(dataRequest.Search) ||
                x.Category.Contains(dataRequest.Search) ||
                x.LogLevel.Contains(dataRequest.Search) ||
                x.Content.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)SystemLogSortEnum.更新時間Descending:
                        DataSource = DataSource.OrderByDescending(x => x.Updatetime);
                        break;
                    case (int)SystemLogSortEnum.更新時間Ascending:
                        DataSource = DataSource.OrderBy(x => x.Updatetime);
                        break;
                    case (int)SystemLogSortEnum.MessageDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Message)
                            .ThenByDescending(x => x.Updatetime);
                        break;
                    case (int)SystemLogSortEnum.MessageAscending:
                        DataSource = DataSource.OrderBy(x => x.Message)
                            .ThenByDescending(x => x.Updatetime);
                        break;
                    case (int)SystemLogSortEnum.CategoryDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Category)
                            .ThenByDescending(x => x.Updatetime);
                        break;
                    case (int)SystemLogSortEnum.CategoryAscending:
                        DataSource = DataSource.OrderBy(x => x.Category)
                            .ThenByDescending(x => x.Updatetime);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<SystemLog>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<SystemLogAdapterModel> adapterModelObjects =
                Mapper.Map<List<SystemLogAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<SystemLogAdapterModel> GetAsync(int id)

        {
            SystemLog item = await context.SystemLog
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            SystemLogAdapterModel result = Mapper.Map<SystemLogAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(SystemLogAdapterModel paraObject)
        {
            try
            {
                SystemLog itemParameter = Mapper.Map<SystemLog>(paraObject);
                CleanTrackingHelper.Clean<SystemLog>(context);
                await context.SystemLog
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<SystemLog>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(SystemLogAdapterModel paraObject)
        {
            try
            {
                SystemLog itemData = Mapper.Map<SystemLog>(paraObject);
                CleanTrackingHelper.Clean<SystemLog>(context);
                SystemLog item = await context.SystemLog
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<SystemLog>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<SystemLog>(context);
                    return VerifyRecordResultFactory.Build(true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "修改記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "修改記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(long id)
        {
            try
            {
                CleanTrackingHelper.Clean<SystemLog>(context);
                SystemLog item = await context.SystemLog
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<SystemLog>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<SystemLog>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(SystemLogAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(SystemLogAdapterModel paraObject)
        {
            var searchItem = await context.SystemLog
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(SystemLogAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            var searchItem = await context.SystemLog
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
        Task OhterDependencyData(SystemLogAdapterModel data)
        {
            return Task.FromResult(0);
        }
        #endregion
    }
}
