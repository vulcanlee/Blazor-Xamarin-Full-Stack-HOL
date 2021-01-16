using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.Helpers;
    using Backend.SortModels;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareBusiness.Factories;
    using ShareBusiness.Helpers;
    using ShareDomain.DataModels;
    using ShareDomain.Enums;

    public class OrderService : IOrderService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public OrderService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<OrderAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<OrderAdapterModel> data = new List<OrderAdapterModel>();
            DataRequestResult<OrderAdapterModel> result = new DataRequestResult<OrderAdapterModel>();
            var DataSource = context.Orders
                .AsNoTracking();
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
                    case (int)OrderSortEnum.OrderDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderDate);
                        break;
                    case (int)OrderSortEnum.OrderDateAscending:
                        DataSource = DataSource.OrderBy(x => x.OrderDate);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<Order>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<OrderAdapterModel> adaptorModelObjects =
                Mapper.Map<List<OrderAdapterModel>>(DataSource);

            foreach (var adaptorModelItem in adaptorModelObjects)
            {
                // ??? 這裡需要完成管理者人員的相關資料讀取程式碼
            }
            #endregion

            result.Result = adaptorModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<OrderAdapterModel> GetAsync(int id)
        {
            Order item = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            OrderAdapterModel result = Mapper.Map<OrderAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(OrderAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<Order>(context);
            Order itemParameter = Mapper.Map<Order>(paraObject);
            CleanTrackingHelper.Clean<Order>(context);
            await context.Orders
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<Order>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(OrderAdapterModel paraObject)
        {
            Order itemData = Mapper.Map<Order>(paraObject);
            CleanTrackingHelper.Clean<Order>(context);
            Order item = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Order>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Order>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<Order>(context);
            Order item = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Order>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Order>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(OrderAdapterModel paraObject)
        {
            var searchItem = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderAdapterModel paraObject)
        {
            var searchItem = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            OrderItem item = await context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
    }
}
