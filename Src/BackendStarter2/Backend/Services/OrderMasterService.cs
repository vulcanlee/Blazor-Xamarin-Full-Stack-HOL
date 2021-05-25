using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareBusiness.Factories;
    using ShareBusiness.Helpers;
    using ShareDomain.DataModels;
    using ShareDomain.Enums;

    public class OrderMasterService : IOrderMasterService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public OrderMasterService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<OrderMasterAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<OrderMasterAdapterModel> data = new List<OrderMasterAdapterModel>();
            DataRequestResult<OrderMasterAdapterModel> result = new DataRequestResult<OrderMasterAdapterModel>();
            var DataSource = context.Order
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
                    case (int)OrderMasterSortEnum.OrderDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderDate);
                        break;
                    case (int)OrderMasterSortEnum.OrderDateAscending:
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
            List<OrderMasterAdapterModel> adapterModelObjects =
                Mapper.Map<List<OrderMasterAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<OrderMasterAdapterModel> GetAsync(int id)
        {
            Order item = await context.Order
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            OrderMasterAdapterModel result = Mapper.Map<OrderMasterAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(OrderMasterAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<Order>(context);
            Order itemParameter = Mapper.Map<Order>(paraObject);
            CleanTrackingHelper.Clean<Order>(context);
            await context.Order
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<Order>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(OrderMasterAdapterModel paraObject)
        {
            Order itemData = Mapper.Map<Order>(paraObject);
            CleanTrackingHelper.Clean<Order>(context);
            Order item = await context.Order
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
            Order item = await context.Order
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(OrderMasterAdapterModel paraObject)
        {
            var searchItem = await context.Order
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderMasterAdapterModel paraObject)
        {
            var searchItem = await context.Order
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderMasterAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            OrderItem item = await context.OrderItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        Task OhterDependencyData(OrderMasterAdapterModel data)
        {
            return Task.FromResult(0);
        }
    }
}
