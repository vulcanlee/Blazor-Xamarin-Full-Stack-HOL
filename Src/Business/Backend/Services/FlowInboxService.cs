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
    using Backend.Helpers;
    using Backend.Models;

    public class FlowInboxService : IFlowInboxService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<FlowInboxService> Logger { get; }
        public UserHelper CurrentUserHelper { get; }
        #endregion

        #region 建構式
        public FlowInboxService(BackendDBContext context, IMapper mapper,
            ILogger<FlowInboxService> logger, UserHelper currentUserHelper)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
            CurrentUserHelper = currentUserHelper;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<FlowInboxAdapterModel>> GetAsync(DataRequest dataRequest,
            UserHelper UserHelper, CurrentUser CurrentUser)
        {
            List<FlowInboxAdapterModel> data = new();
            DataRequestResult<FlowInboxAdapterModel> result = new();
            var DataSource = context.FlowInbox
                .Include(x => x.MyUser)
                .Include(x => x.FlowMaster)
                .AsNoTracking();
        
            #region 進行搜尋動作
            if (!string.IsNullOrWhiteSpace(dataRequest.Search))
            {
                DataSource = DataSource
                .Where(x => x.Title.Contains(dataRequest.Search) ||
                x.Title.Contains(dataRequest.Search));
            }
            #endregion

            #region 決定要顯示那些使用者郵件
            if (CurrentUser.LoginMyUserAdapterModel.Account.ToLower() != MagicHelper.開發者帳號.ToString())
            {
                DataSource = DataSource
                    .Where(x => x.MyUserId == CurrentUser.LoginMyUserAdapterModel.Id);
            }
            #endregion


            #region 進行排序動作
            if (dataRequest.Sorted != null)
            {
                SortCondition CurrentSortCondition = dataRequest.Sorted;
                switch (CurrentSortCondition.Id)
                {
                    case (int)FlowInboxSortEnum.CreateDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.ReceiveTime);
                        break;
                    case (int)FlowInboxSortEnum.CreateDateAscending:
                        DataSource = DataSource.OrderBy(x => x.ReceiveTime);
                        break;
                    default:
                        DataSource = DataSource.OrderBy(x => x.Id);
                        break;
                }
            }
            #endregion

            #region 進行分頁
            // 取得記錄總數量，將要用於分頁元件面板使用
            result.Count = DataSource.Cast<FlowInbox>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowInboxAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowInboxAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<FlowInboxAdapterModel> GetAsync(int id)
        {
            FlowInbox item = await context.FlowInbox
                .Include(x => x.MyUser)
                .Include(x => x.FlowMaster)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            FlowInboxAdapterModel result = Mapper.Map<FlowInboxAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(FlowInboxAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<FlowInbox>(context);
                FlowInbox itemParameter = Mapper.Map<FlowInbox>(paraObject);
                CleanTrackingHelper.Clean<FlowInbox>(context);
                await context.FlowInbox
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();

                CleanTrackingHelper.Clean<FlowInbox>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(FlowInboxAdapterModel paraObject)
        {
            try
            {
                FlowInbox itemData = Mapper.Map<FlowInbox>(paraObject);
                CleanTrackingHelper.Clean<FlowInbox>(context);
                FlowInbox item = await context.FlowInbox
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowInbox>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowInbox>(context);
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
                CleanTrackingHelper.Clean<FlowInbox>(context);
                FlowInbox item = await context.FlowInbox
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowInbox>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowInbox>(context);
                    CleanTrackingHelper.Clean<FlowHistory>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(FlowInboxAdapterModel paraObject)
        {
            await Task.Yield();
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            if (paraObject.FlowMasterId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個簽核申請");
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowInboxAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowInbox>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            if (paraObject.FlowMasterId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個簽核申請");
            }

            var searchItem = await context.FlowInbox
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowInboxAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<OrderItem>(context);
                CleanTrackingHelper.Clean<FlowInbox>(context);

                var searchItem = await context.FlowInbox
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (searchItem == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄_要刪除的紀錄已經不存在資料庫上);
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
        Task OhterDependencyData(FlowInboxAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            data.FlowMasterName = data.FlowMaster.Title;
            return Task.FromResult(0);
        }

        public async Task MailReadedAsync(FlowInboxAdapterModel flowInboxAdapterModel)
        {
            CleanTrackingHelper.Clean<FlowInbox>(context);
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
            flowInboxAdapterModel.IsRead = true;
            var flowInbox = Mapper.Map<FlowInbox>(flowInboxAdapterModel);
            context.FlowInbox.Update(flowInbox);
            await context.SaveChangesAsync();
            CleanTrackingHelper.Clean<FlowInbox>(context);
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
        }
        #endregion
    }
}
