﻿using System.Collections.Generic;
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
    using System.Linq;

    public class CategoryMainService : ICategoryMainService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<CategoryMainService> Logger { get; }
        #endregion

        #region 建構式
        public CategoryMainService(BackendDBContext context, IMapper mapper,
            ILogger<CategoryMainService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<CategoryMainAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<CategoryMainAdapterModel> data = new();
            DataRequestResult<CategoryMainAdapterModel> result = new();
            var DataSource = context.CategoryMain
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
                    case (int)CategoryMainSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)CategoryMainSortEnum.NameAscending:
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
            result.Count = DataSource.Cast<CategoryMain>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<CategoryMainAdapterModel> adapterModelObjects =
                Mapper.Map<List<CategoryMainAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<List<CategoryMainAdapterModel>> GetAsync()
        {
            List<CategoryMainAdapterModel> data = new();
            var DataSource = context.CategoryMain
                .AsNoTracking()
                .OrderBy(x => x.Name);

            var allItems = await DataSource.ToListAsync();
            data = Mapper.Map<List<CategoryMainAdapterModel>>(allItems);

            return data;
        }

        public async Task<CategoryMainAdapterModel> GetAsync(int id)
        {
            CategoryMain item = await context.CategoryMain
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            CategoryMainAdapterModel result = Mapper.Map<CategoryMainAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(CategoryMainAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<CategoryMain>(context);
                CategoryMain itemParameter = Mapper.Map<CategoryMain>(paraObject);
                CleanTrackingHelper.Clean<CategoryMain>(context);
                await context.CategoryMain
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<CategoryMain>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(CategoryMainAdapterModel paraObject)
        {
            try
            {
                CategoryMain itemData = Mapper.Map<CategoryMain>(paraObject);
                CleanTrackingHelper.Clean<CategoryMain>(context);
                CategoryMain item = await context.CategoryMain
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<CategoryMain>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<CategoryMain>(context);
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
                CleanTrackingHelper.Clean<CategoryMain>(context);
                CategoryMain item = await context.CategoryMain
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<CategoryMain>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<CategoryMain>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(CategoryMainAdapterModel paraObject)
        {
            var searchItem = await context.CategoryMain
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == paraObject.Name);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(CategoryMainAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<CategoryMain>(context);
            var searchItem = await context.CategoryMain
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.CategoryMain
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Name == paraObject.Name &&
               x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要修改的紀錄已經存在無法修改);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(CategoryMainAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<OrderItem>(context);
                CleanTrackingHelper.Clean<CategoryMain>(context);

                var searchItem = await context.CategoryMain
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (searchItem == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
                }

                var searchOrderItemItem = await context.CategorySub
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CategoryMainId == paraObject.Id);
                if (searchOrderItemItem != null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.該紀錄無法刪除因為有其他資料表在使用中);
                }
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                return VerifyRecordResultFactory.Build(false, "刪除記錄發生例外異常", ex);
            }
        }
        #endregion

        #region 其他服務方法
        Task OhterDependencyData(CategoryMainAdapterModel data)
        {
            return Task.FromResult(0);
        }
        #endregion

        #region 啟用或停用的紀錄變更
        public async Task DisableIt(CategoryMainAdapterModel paraObject)
        {
            await Task.Delay(100);
            CategoryMain curritem = await context.CategoryMain
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            CleanTrackingHelper.Clean<CategoryMain>(context);
            curritem.Enable = false;
            //context.Entry(curritem).State = EntityState.Modified;
            context.Update(curritem);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<CategoryMain>(context);
            return;
        }
        public async Task EnableIt(CategoryMainAdapterModel paraObject)
        {
            await Task.Delay(100);
            CategoryMain curritem = await context.CategoryMain
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            CleanTrackingHelper.Clean<CategoryMain>(context);
            curritem.Enable = true;
            context.Entry(curritem).State = EntityState.Modified;
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<CategoryMain>(context);
            return;
        }
        #endregion
    }
}
