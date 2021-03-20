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

    public class ProductService : IProductService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public ProductService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<ProductAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<ProductAdapterModel> data = new List<ProductAdapterModel>();
            DataRequestResult<ProductAdapterModel> result = new DataRequestResult<ProductAdapterModel>();
            var DataSource = context.Product
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
                    case (int)ProductSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)ProductSortEnum.NameAscending:
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
            result.Count = DataSource.Cast<Product>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<ProductAdapterModel> adapterModelObjects =
                Mapper.Map<List<ProductAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<ProductAdapterModel> GetAsync(int id)

        {
            Product item = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            ProductAdapterModel result = Mapper.Map<ProductAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(ProductAdapterModel paraObject)
        {
            Product itemParameter = Mapper.Map<Product>(paraObject);
            CleanTrackingHelper.Clean<Product>(context);
            await context.Product
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<Product>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(ProductAdapterModel paraObject)
        {
            Product itemData = Mapper.Map<Product>(paraObject);
            CleanTrackingHelper.Clean<Product>(context);
            Product item = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Product>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Product>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<Product>(context);
            Product item = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<Product>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<Product>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(ProductAdapterModel paraObject)
        {
            var searchItem = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(ProductAdapterModel paraObject)
        {
            var searchItem = await context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(ProductAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            OrderItem item = await context.OrderItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        Task OhterDependencyData(ProductAdapterModel data)
        {
            return Task.FromResult(0);
        }
    }
}
