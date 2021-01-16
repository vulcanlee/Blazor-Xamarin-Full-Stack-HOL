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

    public class LeaveCategoryService : ILeaveCategoryService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public LeaveCategoryService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task<DataRequestResult<LeaveCategoryAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<LeaveCategoryAdapterModel> data = new List<LeaveCategoryAdapterModel>();
            DataRequestResult<LeaveCategoryAdapterModel> result = new DataRequestResult<LeaveCategoryAdapterModel>();
            var DataSource = context.LeaveCategories
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
                    case (int)LeaveCategorySortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)LeaveCategorySortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)LeaveCategorySortEnum.OrderNumberDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderNumber);
                        break;
                    case (int)LeaveCategorySortEnum.OrderNumberAscending:
                        DataSource = DataSource.OrderBy(x => x.OrderNumber);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<LeaveCategory>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<LeaveCategoryAdapterModel> adaptorModelObjects =
                Mapper.Map<List<LeaveCategoryAdapterModel>>(DataSource);

            foreach (var adaptorModelItem in adaptorModelObjects)
            {
                // ??? 這裡需要完成管理者人員的相關資料讀取程式碼
            }
            #endregion

            result.Result = adaptorModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<LeaveCategoryAdapterModel> GetAsync(int id)

        {
            LeaveCategory item = await context.LeaveCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            LeaveCategoryAdapterModel result = Mapper.Map<LeaveCategoryAdapterModel>(item);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(LeaveCategoryAdapterModel paraObject)
        {
            LeaveCategory itemParameter = Mapper.Map<LeaveCategory>(paraObject);
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            await context.LeaveCategories
                .AddAsync(itemParameter);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> UpdateAsync(LeaveCategoryAdapterModel paraObject)
        {
            LeaveCategory itemData = Mapper.Map<LeaveCategory>(paraObject);
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            LeaveCategory item = await context.LeaveCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<LeaveCategory>(context);
                context.Entry(itemData).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<LeaveCategory>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }

        public async Task<VerifyRecordResult> DeleteAsync(int id)
        {
            CleanTrackingHelper.Clean<LeaveCategory>(context);
            LeaveCategory item = await context.LeaveCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
            }
            else
            {
                CleanTrackingHelper.Clean<LeaveCategory>(context);
                context.Entry(item).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<LeaveCategory>(context);
                return VerifyRecordResultFactory.Build(true);
            }
        }
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(LeaveCategoryAdapterModel paraObject)
        {
            var searchItem = await context.LeaveCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(LeaveCategoryAdapterModel paraObject)
        {
            var searchItem = await context.LeaveCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }
        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(LeaveCategoryAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<LeaveForm>(context);
            LeaveForm item = await context.LeaveForms
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LeaveCategoryId == paraObject.Id);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
            }
            return VerifyRecordResultFactory.Build(true);
        }
    }
}
