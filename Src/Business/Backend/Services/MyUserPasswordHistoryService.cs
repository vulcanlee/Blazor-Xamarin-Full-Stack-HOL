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

    public class MyUserPasswordHistoryService : IMyUserPasswordHistoryService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<MyUserPasswordHistoryService> Logger { get; }
        #endregion

        #region 建構式
        public MyUserPasswordHistoryService(BackendDBContext context, IMapper mapper,
            ILogger<MyUserPasswordHistoryService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<MyUserPasswordHistoryAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<MyUserPasswordHistoryAdapterModel> data = new();
            DataRequestResult<MyUserPasswordHistoryAdapterModel> result = new();
            var DataSource = context.MyUserPasswordHistory
                .AsNoTracking()
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeDescending:
                        DataSource = DataSource.OrderByDescending(x => x.ChangePasswordDatetime);
                        break;
                    case (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeAscending:
                        DataSource = DataSource.OrderBy(x => x.ChangePasswordDatetime);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<MyUserPasswordHistory>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MyUserPasswordHistoryAdapterModel> adapterModelObjects =
                Mapper.Map<List<MyUserPasswordHistoryAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<MyUserPasswordHistoryAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<MyUserPasswordHistoryAdapterModel> data = new();
            DataRequestResult<MyUserPasswordHistoryAdapterModel> result = new();
            var DataSource = context.MyUserPasswordHistory
                .AsNoTracking()
                .Where(x => x.MyUserId == id);
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeDescending:
                        DataSource = DataSource.OrderByDescending(x => x.ChangePasswordDatetime);
                        break;
                    case (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeAscending:
                        DataSource = DataSource.OrderBy(x => x.ChangePasswordDatetime);
                        break;
                    default:
                        DataSource = DataSource.OrderByDescending(x => x.ChangePasswordDatetime);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<MyUserPasswordHistory>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<MyUserPasswordHistoryAdapterModel> adapterModelObjects =
                Mapper.Map<List<MyUserPasswordHistoryAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<MyUserPasswordHistoryAdapterModel> GetAsync(int id)
        {
            MyUserPasswordHistory item = await context.MyUserPasswordHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            MyUserPasswordHistoryAdapterModel result = Mapper.Map<MyUserPasswordHistoryAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(MyUserPasswordHistoryAdapterModel paraObject)
        {
            try
            {
                MyUserPasswordHistory itemParameter = Mapper.Map<MyUserPasswordHistory>(paraObject);
                CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                await context.MyUserPasswordHistory
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(MyUserPasswordHistoryAdapterModel paraObject)
        {
            try
            {
                MyUserPasswordHistory itemData = Mapper.Map<MyUserPasswordHistory>(paraObject);
                CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                MyUserPasswordHistory item = await context.MyUserPasswordHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
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
                CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                MyUserPasswordHistory item = await context.MyUserPasswordHistory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserPasswordHistoryAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserPasswordHistoryAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserPasswordHistoryAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(MyUserPasswordHistoryAdapterModel data)
        {
            return Task.FromResult(0);
        }
        #endregion
    }
}
