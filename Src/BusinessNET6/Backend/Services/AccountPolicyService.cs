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

    public class AccountPolicyService : IAccountPolicyService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<AccountPolicyService> Logger { get; }
        #endregion

        #region 建構式
        public AccountPolicyService(BackendDBContext context, IMapper mapper,
            ILogger<AccountPolicyService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region CRUD 服務
        public async Task<AccountPolicyAdapterModel> GetAsync()
        {
            var result = await context.AccountPolicy
                  .AsNoTracking()
                  .OrderBy(x => x.Id)
                  .FirstOrDefaultAsync();
            var resultAdapterModel = Mapper.Map<AccountPolicyAdapterModel>(result);
            return resultAdapterModel;
        }

        public async Task<DataRequestResult<AccountPolicyAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<AccountPolicyAdapterModel> data = new();
            DataRequestResult<AccountPolicyAdapterModel> result = new();
            var DataSource = context.AccountPolicy
                .AsNoTracking();

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<AccountPolicy>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<AccountPolicyAdapterModel> adapterModelObjects =
                Mapper.Map<List<AccountPolicyAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<AccountPolicyAdapterModel> GetAsync(int id)

        {
            AccountPolicy item = await context.AccountPolicy
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            AccountPolicyAdapterModel result = Mapper.Map<AccountPolicyAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(AccountPolicyAdapterModel paraObject)
        {
            try
            {
                AccountPolicy itemParameter = Mapper.Map<AccountPolicy>(paraObject);
                CleanTrackingHelper.Clean<AccountPolicy>(context);
                await context.AccountPolicy
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();
                CleanTrackingHelper.Clean<AccountPolicy>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(AccountPolicyAdapterModel paraObject)
        {
            try
            {
                AccountPolicy itemData = Mapper.Map<AccountPolicy>(paraObject);
                CleanTrackingHelper.Clean<AccountPolicy>(context);
                AccountPolicy item = await context.AccountPolicy
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AccountPolicy>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AccountPolicy>(context);
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
                CleanTrackingHelper.Clean<AccountPolicy>(context);
                AccountPolicy item = await context.AccountPolicy
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<AccountPolicy>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<AccountPolicy>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(AccountPolicyAdapterModel paraObject)
        {
            var searchItem = await context.AccountPolicy
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (searchItem != null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要新增的紀錄已經存在無法新增);
            }
            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(AccountPolicyAdapterModel paraObject)
        {
            var searchItem = await context.AccountPolicy
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(AccountPolicyAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<OrderItem>(context);
            var searchItem = await context.AccountPolicy
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
        Task OhterDependencyData(AccountPolicyAdapterModel data)
        {
            return Task.FromResult(0);
        }
        #endregion
    }
}
