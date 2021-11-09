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

    public class PolicyDetailService : IPolicyDetailService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<PolicyDetailService> Logger { get; }
        #endregion

        #region 建構式
        public PolicyDetailService(BackendDBContext context, IMapper mapper,
            ILogger<PolicyDetailService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<PolicyDetailAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<PolicyDetailAdapterModel> data = new();
            DataRequestResult<PolicyDetailAdapterModel> result = new();
            var DataSource = context.PolicyDetail
                .AsNoTracking()
                .Include(x => x.PolicyHeader)
                .Include(x => x.MyUser)
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
                    case (int)PolicyDetailSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)PolicyDetailSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)PolicyDetailSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)PolicyDetailSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    case (int)PolicyDetailSortEnum.EnableDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Enable);
                        break;
                    case (int)PolicyDetailSortEnum.EnableAscending:
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
            result.Count = DataSource.Cast<PolicyDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<PolicyDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<PolicyDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);

            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<DataRequestResult<PolicyDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest)
        {
            List<PolicyDetailAdapterModel> data = new();
            DataRequestResult<PolicyDetailAdapterModel> result = new();
            var DataSource = context.PolicyDetail
                .AsNoTracking()
                .Include(x => x.PolicyHeader)
                .Include(x => x.MyUser)
                .Where(x => x.PolicyHeaderId == id);

            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Name.Contains(dataRequest.Search) ||
                x.PolicyHeader.Name.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)PolicyDetailSortEnum.NameDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Name);
                        break;
                    case (int)PolicyDetailSortEnum.NameAscending:
                        DataSource = DataSource.OrderBy(x => x.Name);
                        break;
                    case (int)PolicyDetailSortEnum.LevelDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Level);
                        break;
                    case (int)PolicyDetailSortEnum.LevelAscending:
                        DataSource = DataSource.OrderBy(x => x.Level);
                        break;
                    case (int)PolicyDetailSortEnum.EnableDescending:
                        DataSource = DataSource.OrderByDescending(x => x.Enable);
                        break;
                    case (int)PolicyDetailSortEnum.EnableAscending:
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
            result.Count = DataSource.Cast<PolicyDetail>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<PolicyDetailAdapterModel> adapterModelObjects =
                Mapper.Map<List<PolicyDetailAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<PolicyDetailAdapterModel> GetAsync(int id)
        {
            PolicyDetail item = await context.PolicyDetail
                .AsNoTracking()
                .Include(x => x.PolicyHeader)
                .Include(x => x.MyUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            PolicyDetailAdapterModel result = Mapper.Map<PolicyDetailAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(PolicyDetailAdapterModel paraObject)
        {
            try
            {
                PolicyDetail itemParameter = Mapper.Map<PolicyDetail>(paraObject);
                CleanTrackingHelper.Clean<PolicyDetail>(context);
                await context.PolicyDetail
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<PolicyDetail>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(PolicyDetailAdapterModel paraObject)
        {
            try
            {
                PolicyDetail itemData = Mapper.Map<PolicyDetail>(paraObject);
                CleanTrackingHelper.Clean<PolicyDetail>(context);
                PolicyDetail item = await context.PolicyDetail
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<PolicyDetail>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<PolicyDetail>(context);
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
                CleanTrackingHelper.Clean<PolicyDetail>(context);
                PolicyDetail item = await context.PolicyDetail
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<PolicyDetail>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<PolicyDetail>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(PolicyDetailAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<PolicyDetail>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            var item = await context.PolicyDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PolicyHeaderId == paraObject.PolicyHeaderId &&
                x.MyUserId == paraObject.MyUserId);
            if (item != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(PolicyDetailAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<PolicyDetail>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }

            var searchItem = await context.PolicyDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            searchItem = await context.PolicyDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PolicyHeaderId == paraObject.PolicyHeaderId &&
                x.MyUserId == paraObject.MyUserId &&
                x.Id != paraObject.Id);
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, "同一個簽核政策內，使用者不能重複指定");
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(PolicyDetailAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<PolicyDetail>(context);
            var searchItem = await context.PolicyDetail
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
        Task OhterDependencyData(PolicyDetailAdapterModel data)
        {
            data.PolicyHeaderName = data.PolicyHeader.Name;
            data.MyUserName = data.MyUser.Name;
            return Task.FromResult(0);
        }
        #endregion

        #region 啟用或停用的紀錄變更
        public async Task DisableIt(PolicyDetailAdapterModel paraObject)
        {
            PolicyDetail itemData = Mapper.Map<PolicyDetail>(paraObject);
            CleanTrackingHelper.Clean<PolicyDetail>(context);
            PolicyDetail item = await context.PolicyDetail
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

        public async Task EnableIt(PolicyDetailAdapterModel paraObject)
        {
            PolicyDetail itemData = Mapper.Map<PolicyDetail>(paraObject);
            CleanTrackingHelper.Clean<PolicyDetail>(context);
            PolicyDetail item = await context.PolicyDetail
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
