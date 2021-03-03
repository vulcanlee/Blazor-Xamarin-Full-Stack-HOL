using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using ShareBusiness.Helpers;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareDomain.DataModels;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using AutoMapper;
    using ShareBusiness.Factories;
    using ShareDomain.Enums;

    public class TravelExpenseService : ITravelExpenseService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public TravelExpenseService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<TravelExpenseAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<TravelExpenseAdapterModel> data = new List<TravelExpenseAdapterModel>();
            DataRequestResult<TravelExpenseAdapterModel> result = new DataRequestResult<TravelExpenseAdapterModel>();
            var DataSource = context.TravelExpense
                .AsNoTracking()
                .Include(x => x.MyUser)
                .AsQueryable();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.MyUser.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)TravelExpenseSortEnum.ApplyDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.ApplyDate);
                        break;
                    case (int)TravelExpenseSortEnum.ApplyDateAscending:
                        DataSource = DataSource.OrderBy(x => x.ApplyDate);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<TravelExpense>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<TravelExpenseAdapterModel> adapterModelObjects =
                Mapper.Map<List<TravelExpenseAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<TravelExpenseAdapterModel> GetAsync(int id)

        {
            TravelExpense item = await context.TravelExpense
                .AsNoTracking()
                .Include(x => x.MyUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            TravelExpenseAdapterModel result = Mapper.Map<TravelExpenseAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(TravelExpenseAdapterModel paraObject)
        {
            TravelExpense itemParameter = Mapper.Map<TravelExpense>(paraObject);
            CleanTrackingHelper.Clean<TravelExpense>(context);
            await context.TravelExpense
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<TravelExpense>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(TravelExpenseAdapterModel paraObject)
        {
            TravelExpense itemData = Mapper.Map<TravelExpense>(paraObject);
            CleanTrackingHelper.Clean<TravelExpense>(context);
            TravelExpense item = await context.TravelExpense
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<TravelExpense>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<TravelExpense>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<TravelExpense>(context);
            TravelExpense item = await context.TravelExpense
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<TravelExpense>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<TravelExpense>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(TravelExpenseAdapterModel paraObject)
        {
            var searchMyUserItem = await context.MyUser
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.MyUserId);
            if (searchMyUserItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.需要指定使用者);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(TravelExpenseAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(TravelExpenseAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
            TravelExpenseDetail item = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TravelExpenseId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        Task OhterDependencyData(TravelExpenseAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            return Task.FromResult(0);
        }
    }
}
