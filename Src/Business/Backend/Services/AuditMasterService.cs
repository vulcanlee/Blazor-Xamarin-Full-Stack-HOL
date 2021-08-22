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

    public class AuditMasterService : IAuditMasterService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<AuditMasterService> Logger { get; }
        #endregion

        #region 建構式
        public AuditMasterService(BackendDBContext context, IMapper mapper,
            ILogger<AuditMasterService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<AuditMasterAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<AuditMasterAdapterModel> data = new();
            DataRequestResult<AuditMasterAdapterModel> result = new();
            var DataSource = context.AuditMaster
                .Include(x => x.MyUser)
                .Include(x => x.PolicyHeader)
                .Include(x => x.AuditUser)
                .AsNoTracking();
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Title.Contains(dataRequest.Search) ||
                x.Title.Contains(dataRequest.Search));
            }
            #endregion

            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)AuditMasterSortEnum.CreateDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.CreateDate);
                        break;
                    case (int)AuditMasterSortEnum.CreateDateAscending:
                        DataSource = DataSource.OrderBy(x => x.CreateDate);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<AuditMaster>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<AuditMasterAdapterModel> adapterModelObjects =
                Mapper.Map<List<AuditMasterAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<AuditMasterAdapterModel> GetAsync(int id)
        {
            AuditMaster item = await context.AuditMaster
                .Include(x => x.MyUser)
                .Include(x => x.PolicyHeader)
                .Include(x => x.AuditUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            AuditMasterAdapterModel result = Mapper.Map<AuditMasterAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(AuditMasterAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<AuditMaster>(context);
                AuditMaster itemParameter = Mapper.Map<AuditMaster>(paraObject);
                CleanTrackingHelper.Clean<AuditMaster>(context);
                await context.AuditMaster
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<AuditMaster>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(AuditMasterAdapterModel paraObject)
        {
            try
            {
                AuditMaster itemData = Mapper.Map<AuditMaster>(paraObject);
                CleanTrackingHelper.Clean<AuditMaster>(context);
                AuditMaster item = await context.AuditMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AuditMaster>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AuditMaster>(context);
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
                CleanTrackingHelper.Clean<AuditMaster>(context);
                AuditMaster item = await context.AuditMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AuditMaster>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AuditMaster>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(AuditMasterAdapterModel paraObject)
        {
            await Task.Yield();
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            if (paraObject.PolicyHeaderId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個簽核政策");
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(AuditMasterAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<AuditMaster>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            if (paraObject.PolicyHeaderId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個簽核政策");
            }

            var searchItem = await context.AuditMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(AuditMasterAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<OrderItem>(context);
                CleanTrackingHelper.Clean<AuditMaster>(context);

                var searchItem = await context.AuditMaster
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (searchItem == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
                }

                var searchOrderItemItem = await context.AuditUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AuditMasterId == paraObject.Id);
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
        Task OhterDependencyData(AuditMasterAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            data.PolicyHeaderName = data.PolicyHeader.Name;
            return Task.FromResult(0);
        }
        #endregion
    }
}
