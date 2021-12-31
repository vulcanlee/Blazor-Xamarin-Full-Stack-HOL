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

    public class OrderItemService : IOrderItemService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<OrderItemService> Logger { get; }
        #endregion

        #region 建構式
        public OrderItemService(BackendDBContext context, IMapper mapper,
            ILogger<OrderItemService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<OrderItemAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<OrderItemAdapterModel> data = new();
            DataRequestResult<OrderItemAdapterModel> result = new();
            var DataSource = context.OrderItem
                .AsNoTracking()
                .Include(x => x.Product)
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
                    case (int)OrderItemSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)OrderItemSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<OrderItem>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<OrderItemAdapterModel> adapterModelObjects =
                Mapper.Map<List<OrderItemAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<OrderItemAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<OrderItemAdapterModel> data = new();
            DataRequestResult<OrderItemAdapterModel> result = new();
            var DataSource = context.OrderItem
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.OrderMasterId == id);
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.Product.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)OrderItemSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)OrderItemSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<OrderItem>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<OrderItemAdapterModel> adapterModelObjects =
                Mapper.Map<List<OrderItemAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<OrderItemAdapterModel> GetAsync(int id)
        {
            OrderItem item = await context.OrderItem
                .AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            OrderItemAdapterModel result = Mapper.Map<OrderItemAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(OrderItemAdapterModel paraObject)
        {
            try
            {
                OrderItem itemParameter = Mapper.Map<OrderItem>(paraObject);
                CleanTrackingHelper.Clean<OrderItem>(context);
                await context.OrderItem
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<OrderItem>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(OrderItemAdapterModel paraObject)
        {
            try
            {
                OrderItem itemData = Mapper.Map<OrderItem>(paraObject);
                CleanTrackingHelper.Clean<OrderItem>(context);
                OrderItem item = await context.OrderItem
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<OrderItem>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<OrderItem>(context);
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
                CleanTrackingHelper.Clean<OrderItem>(context);
                OrderItem item = await context.OrderItem
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<OrderItem>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<OrderItem>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(OrderItemAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            if (paraObject.ProductId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入該訂單要用到的產品);
            }
            var item = await context.OrderItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderMasterId == paraObject.OrderMasterId &&
                x.ProductId == paraObject.ProductId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該訂單已經存在該產品_不能重複同樣的商品在一訂單內);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderItemAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            if (paraObject.ProductId == 0)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.尚未輸入該訂單要用到的產品);
            }

            var searchItem = await context.OrderItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.OrderItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderMasterId == paraObject.OrderMasterId &&
                x.ProductId == paraObject.ProductId &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該訂單已經存在該產品_不能重複同樣的商品在一訂單內);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderItemAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            var searchItem = await context.OrderItem
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
        Task OhterDependencyData(OrderItemAdapterModel data)
        {
            data.ProductName = data.Product.Name;
            return Task.FromResult(0);
        }
        #endregion
    }
}
