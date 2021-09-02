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

    public class CategorySubService : ICategorySubService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<CategorySubService> Logger { get; }
        #endregion

        #region 建構式
        public CategorySubService(BackendDBContext context, IMapper mapper,
            ILogger<CategorySubService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<CategorySubAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<CategorySubAdapterModel> data = new();
            DataRequestResult<CategorySubAdapterModel> result = new();
            var DataSource = context.CategorySub
                .AsNoTracking()
                .Include(x => x.CategoryMain)
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
                    case (int)CategorySubSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)CategorySubSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)CategorySubSortEnum.OrderNumberDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderNumber);
                        break;
                    case (int)CategorySubSortEnum.OrderNumberAscending:
                        DataSource = DataSource.OrderBy(x => x.OrderNumber);
                        break;
                    case (int)CategorySubSortEnum.EnableDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Enable);
                        break;
                    case (int)CategorySubSortEnum.EnableAscending:
                        DataSource = DataSource.OrderBy(x => x.Enable);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<CategorySub>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<CategorySubAdapterModel> adapterModelObjects =
                Mapper.Map<List<CategorySubAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<CategorySubAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<CategorySubAdapterModel> data = new();
            DataRequestResult<CategorySubAdapterModel> result = new();
            var DataSource = context.CategorySub
                .AsNoTracking()
                .Include(x => x.CategoryMain)
                .Where(x => x.CategoryMainId == id);

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.CategoryMain.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)CategorySubSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)CategorySubSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)CategorySubSortEnum.OrderNumberDescending:
                        DataSource = DataSource.OrderByDescending(x => x.OrderNumber);
                        break;
                    case (int)CategorySubSortEnum.OrderNumberAscending:
                        DataSource = DataSource.OrderBy(x => x.OrderNumber);
                        break;
                    case (int)CategorySubSortEnum.EnableDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Enable);
                        break;
                    case (int)CategorySubSortEnum.EnableAscending:
                        DataSource = DataSource.OrderBy(x => x.Enable);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<CategorySub>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<CategorySubAdapterModel> adapterModelObjects =
                Mapper.Map<List<CategorySubAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<CategorySubAdapterModel> GetAsync(int id)
        {
            CategorySub item = await context.CategorySub
                .AsNoTracking()
                .Include(x => x.CategoryMain)
                .FirstOrDefaultAsync(x => x.Id == id);
            CategorySubAdapterModel result = Mapper.Map<CategorySubAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(CategorySubAdapterModel paraObject)
        {
            try
            {
                CategorySub itemParameter = Mapper.Map<CategorySub>(paraObject);
                CleanTrackingHelper.Clean<CategorySub>(context);
                await context.CategorySub
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<CategorySub>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(CategorySubAdapterModel paraObject)
        {
            try
            {
                CategorySub itemData = Mapper.Map<CategorySub>(paraObject);
                CleanTrackingHelper.Clean<CategorySub>(context);
                CategorySub item = await context.CategorySub
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<CategorySub>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<CategorySub>(context);
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
                CleanTrackingHelper.Clean<CategorySub>(context);
                CategorySub item = await context.CategorySub
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<CategorySub>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<CategorySub>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(CategorySubAdapterModel paraObject)
        {
            await Task.Yield();
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(CategorySubAdapterModel paraObject)
        {
            var searchItem = await context.CategorySub
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(CategorySubAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<CategorySub>(context);
            var searchItem = await context.CategorySub
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
        Task OhterDependencyData(CategorySubAdapterModel data)
        {
            data.CategoryMainName = data.CategoryMain.Name;
            return Task.FromResult(0);
        }
        #endregion

        #region 啟用或停用的紀錄變更
        public async Task DisableIt(CategorySubAdapterModel paraObject)
        {
            CategorySub itemData = Mapper.Map<CategorySub>(paraObject);
            CleanTrackingHelper.Clean<CategorySub>(context);
            CategorySub item = await context.CategorySub
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = false;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }

        public async Task EnableIt(CategorySubAdapterModel paraObject)
        {
            CategorySub itemData = Mapper.Map<CategorySub>(paraObject);
            CleanTrackingHelper.Clean<CategorySub>(context);
            CategorySub item = await context.CategorySub
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (item == null)
            {
            }
            else
            {
                item.Enable = true;
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<MenuData>(context);
            }
        }
        #endregion
    }
}
