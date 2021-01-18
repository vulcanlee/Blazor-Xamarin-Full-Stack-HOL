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

    public class TravelExpenseDetailService : ITravelExpenseDetailService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public TravelExpenseDetailService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<TravelExpenseDetailAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<TravelExpenseDetailAdapterModel> data = new List<TravelExpenseDetailAdapterModel>();
            DataRequestResult<TravelExpenseDetailAdapterModel> result = new DataRequestResult<TravelExpenseDetailAdapterModel>();
            var DataSource = context.TravelExpenseDetail
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Title.Contains(dataRequest.Search) ||
                x.Summary.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)TravelExpenseDetailSortEnum.TitleDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Title);
                        break;
                    case (int)TravelExpenseDetailSortEnum.TitleAscending:
                        DataSource = DataSource.OrderBy(x => x.Title);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<TravelExpenseDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<TravelExpenseDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<TravelExpenseDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                // ??? 這裡需要完成管理者人員的相關資料讀取程式碼
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<TravelExpenseDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<TravelExpenseDetailAdapterModel> data = new List<TravelExpenseDetailAdapterModel>();
            DataRequestResult<TravelExpenseDetailAdapterModel> result = new DataRequestResult<TravelExpenseDetailAdapterModel>();
            var DataSource = context.TravelExpenseDetail
                .AsNoTracking()
                .Where(x => x.TravelExpenseId == id);

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<TravelExpenseDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<TravelExpenseDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<TravelExpenseDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<TravelExpenseDetailAdapterModel> GetAsync(int id)

        {
            TravelExpenseDetail item = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            TravelExpenseDetailAdapterModel result = Mapper.Map<TravelExpenseDetailAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(TravelExpenseDetailAdapterModel paraObject)
        {
            TravelExpenseDetail itemParameter = Mapper.Map<TravelExpenseDetail>(paraObject);
            CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
            await context.TravelExpenseDetail
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
            await CountingExpense(itemParameter.TravelExpenseId);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(TravelExpenseDetailAdapterModel paraObject)
        {
            TravelExpenseDetail itemData = Mapper.Map<TravelExpenseDetail>(paraObject);
            CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
            TravelExpenseDetail item = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
                await CountingExpense(itemData.TravelExpenseId);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
            TravelExpenseDetail item = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<TravelExpenseDetail>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(TravelExpenseDetailAdapterModel paraObject)
        {
            var searchItem = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == paraObject.Title);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(TravelExpenseDetailAdapterModel paraObject)
        {
            var searchItem = await context.TravelExpenseDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == paraObject.Title &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(TravelExpenseDetailAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task CountingExpense(int TravelExpenseId)
        {
            CleanTrackingHelper.Clean<TravelExpense>(context);
            var totalHours = await context.TravelExpenseDetail
                .AsNoTracking()
                .Where(x => x.TravelExpenseId == TravelExpenseId)
                .SumAsync(x => x.Expense);
            var item = await context.TravelExpense
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == TravelExpenseId);
            if (item != null)
            {
                item.TotalExpense = totalHours;
                context.Update(item);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<TravelExpense>(context);
            }
        }
    }
}
