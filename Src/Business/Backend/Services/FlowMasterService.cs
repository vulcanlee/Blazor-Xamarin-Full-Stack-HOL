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
    using EFCore.BulkExtensions;
    using Backend.Models;

    public class FlowMasterService : IFlowMasterService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<FlowMasterService> Logger { get; }
        public UserHelper UserHelper { get; }
        #endregion

        #region 建構式
        public FlowMasterService(BackendDBContext context, IMapper mapper,
            ILogger<FlowMasterService> logger, UserHelper currentUserHelper)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
            UserHelper = currentUserHelper;
        }
        #endregion

        #region CRUD 服務
        public async Task<DataRequestResult<FlowMasterAdapterModel>> GetAsync(DataRequest dataRequest)
        {
            List<FlowMasterAdapterModel> data = new();
            DataRequestResult<FlowMasterAdapterModel> result = new();
            var DataSource = context.FlowMaster
                .Include(x => x.MyUser)
                .Include(x => x.PolicyHeader)
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
                    case (int)FlowMasterSortEnum.CreateDateDescending:
                        DataSource = DataSource.OrderByDescending(x => x.CreateDate);
                        break;
                    case (int)FlowMasterSortEnum.CreateDateAscending:
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
            result.Count = DataSource.Cast<FlowMaster>().Count();
            DataSource = DataSource.Skip(dataRequest.Skip);
            if (dataRequest.Take != 0)
            {
                DataSource = DataSource.Take(dataRequest.Take);
            }
            #endregion

            #region 在這裡進行取得資料與與額外屬性初始化
            List<FlowMasterAdapterModel> adapterModelObjects =
                Mapper.Map<List<FlowMasterAdapterModel>>(DataSource);

            foreach (var adapterModelItem in adapterModelObjects)
            {
                await OhterDependencyData(adapterModelItem);
            }
            #endregion

            result.Result = adapterModelObjects;
            await Task.Yield();
            return result;
        }

        public async Task<FlowMasterAdapterModel> GetAsync(int id)
        {
            FlowMaster item = await context.FlowMaster
                .Include(x => x.MyUser)
                .Include(x => x.PolicyHeader)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            FlowMasterAdapterModel result = Mapper.Map<FlowMasterAdapterModel>(item);
            await OhterDependencyData(result);
            return result;
        }

        public async Task<VerifyRecordResult> AddAsync(FlowMasterAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<FlowMaster>(context);
                CleanTrackingHelper.Clean<FlowUser>(context);
                CleanTrackingHelper.Clean<FlowHistory>(context);
                CleanTrackingHelper.Clean<MyUser>(context);
                FlowMaster itemParameter = Mapper.Map<FlowMaster>(paraObject);
                CleanTrackingHelper.Clean<FlowMaster>(context);
                await context.FlowMaster
                    .AddAsync(itemParameter);
                await context.SaveChangesAsync();

                #region 產生要審核的使用者清單
                var user = await context.MyUser
                    .FirstOrDefaultAsync(x => x.Id == itemParameter.MyUserId);
                FlowUser auditUser = new FlowUser()
                {
                    MyUserId = itemParameter.MyUserId,
                    FlowMasterId = itemParameter.Id,
                    Enable = true,
                    Level = 0,
                    OnlyCC = false,
                    Name = "發文者",
                };
                await context.FlowUser.AddAsync(auditUser);
                var policyDetails = await context.PolicyDetail
                    .Where(x => x.PolicyHeaderId == paraObject.PolicyHeaderId)
                    .OrderBy(x => x.Level)
                    .ToListAsync();
                foreach (var item in policyDetails)
                {
                    if (item.Enable == false) continue;
                    auditUser = new FlowUser()
                    {
                        MyUserId = item.MyUserId,
                        FlowMasterId = itemParameter.Id,
                        Enable = true,
                        Level = item.Level,
                        OnlyCC = item.OnlyCC,
                        Name = item.Name,
                    };
                    await context.FlowUser.AddAsync(auditUser);
                }

                itemParameter.NextMyUserName = user.Name;
                context.FlowMaster.Update(itemParameter);
                await context.SaveChangesAsync();
                #endregion

                #region 增加簽核流程歷史紀錄 - 建立簽核表單
                FlowHistory history = new FlowHistory()
                {
                    FlowMasterId = itemParameter.Id,
                    MyUserId = itemParameter.MyUserId,
                    Approve = true,
                    Summary = $"{user.Account} / {user.Name} 建立簽核表單",
                    Comment = $"簽核單草稿",
                    Updatetime = DateTime.Now,
                };
                await context.FlowHistory.AddAsync(history);
                await context.SaveChangesAsync();
                #endregion

                CleanTrackingHelper.Clean<FlowMaster>(context);
                CleanTrackingHelper.Clean<FlowUser>(context);
                CleanTrackingHelper.Clean<FlowHistory>(context);
                CleanTrackingHelper.Clean<MyUser>(context);
                return VerifyRecordResultFactory.Build(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "新增記錄發生例外異常");
                return VerifyRecordResultFactory.Build(false, "新增記錄發生例外異常", ex);
            }
        }

        public async Task<VerifyRecordResult> UpdateAsync(FlowMasterAdapterModel paraObject)
        {
            try
            {
                FlowMaster itemData = Mapper.Map<FlowMaster>(paraObject);
                CleanTrackingHelper.Clean<FlowMaster>(context);
                FlowMaster item = await context.FlowMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法修改紀錄);
                }
                else
                {
                    CleanTrackingHelper.Clean<FlowMaster>(context);
                    context.Entry(itemData).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowMaster>(context);
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
                CleanTrackingHelper.Clean<FlowMaster>(context);
                FlowMaster item = await context.FlowMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.無法刪除紀錄);
                }
                else
                {
                    #region 刪除其他關連紀錄
                    CleanTrackingHelper.Clean<FlowUser>(context);
                    CleanTrackingHelper.Clean<FlowHistory>(context);
                    var auditUsers = await context.FlowUser
                        .Where(x => x.FlowMasterId == id)
                        .ToListAsync();
                    context.FlowUser.RemoveRange(auditUsers);
                    var auditHistories = await context.FlowHistory
                        .Where(x => x.FlowMasterId == id)
                        .ToListAsync();
                    context.FlowHistory.RemoveRange(auditHistories);
                    await context.SaveChangesAsync();
                    #endregion

                    CleanTrackingHelper.Clean<FlowMaster>(context);
                    context.Entry(item).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    CleanTrackingHelper.Clean<FlowMaster>(context);
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
        public async Task<VerifyRecordResult> BeforeAddCheckAsync(FlowMasterAdapterModel paraObject)
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

        public async Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowMasterAdapterModel paraObject)
        {
            CleanTrackingHelper.Clean<FlowMaster>(context);
            if (paraObject.MyUserId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個使用者");
            }
            if (paraObject.PolicyHeaderId == 0)
            {
                return VerifyRecordResultFactory.Build(false, "需要指定一個簽核政策");
            }

            var searchItem = await context.FlowMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == paraObject.Id);
            if (searchItem == null)
            {
                return VerifyRecordResultFactory.Build(false, ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上);
            }

            return VerifyRecordResultFactory.Build(true);
        }

        public async Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowMasterAdapterModel paraObject)
        {
            try
            {
                CleanTrackingHelper.Clean<FlowMaster>(context);
                CleanTrackingHelper.Clean<FlowMaster>(context);

                var searchItem = await context.FlowMaster
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
        Task OhterDependencyData(FlowMasterAdapterModel data)
        {
            data.MyUserName = data.MyUser.Name;
            data.PolicyHeaderName = data.PolicyHeader.Name;
            data.GetFlowName();
            return Task.FromResult(0);
        }
        #endregion

        #region 審核動作事件
        #region 取得現在使用者與簽核流程所有使用者
        public async Task<(List<FlowUser> flowUsers, MyUserAdapterModel user)>
            GetUsersDataAsync(FlowMasterAdapterModel flowMasterAdapterModel)
        {
            var user = await UserHelper.GetCurrentUserAsync();

            var flowUsers = await context.FlowUser
                .Include(x => x.MyUser)
                .OrderBy(x => x.Level)
                .Where(x => x.FlowMasterId == flowMasterAdapterModel.Id)
                .ToListAsync();
            return (flowUsers, user);
        }
        #endregion

        #region 副本使用者，自動視為完成審核
        public void CopyUserAutoCompletion(List<FlowUser> flowUsers, int processLevel)
        {
            var flowUserCCs = flowUsers.Where(x => x.Level == processLevel
            && x.OnlyCC == true).ToList();
            foreach (var item in flowUserCCs)
            {
                item.Completion = true;
            }
            return;
        }
        #endregion

        #region 找出一個下一個使用者名稱
        public void FindNextActionUser(List<FlowUser> flowUsers, FlowMasterAdapterModel flowMasterAdapterModel)
        {
            var flowUserNexts = flowUsers.Where(x => x.Level == flowMasterAdapterModel.ProcessLevel
            && x.OnlyCC == false && x.Completion == false).ToList();
            if (flowUserNexts.Count() == 0)
            {
                flowMasterAdapterModel.NextMyUserName = "";
            }
            else
            {
                flowMasterAdapterModel.NextMyUserName = flowUserNexts.Last().MyUser.Name;
            }
            return;
        }
        #endregion

        #region 撤銷流程使用者的完成狀態
        public void RecoveryCompletion(List<FlowUser> flowUsers, int processLevel)
        {
            var flowUserLevel = flowUsers.Where(x => x.Level == processLevel).ToList();
            foreach (var item in flowUserLevel)
            {
                item.Completion = false;
            }
            return;
        }
        #endregion

        #region 加入批示歷史紀錄
        public async Task AddHistoryRecord(MyUserAdapterModel myUserAdapterModel,
            FlowMasterAdapterModel flowMasterAdapterModel, string Summary, string Comment,
            bool approve)
        {
            FlowHistory history = new FlowHistory()
            {
                Comment = Comment,
                Summary = Summary,
                Updatetime = DateTime.Now,
                FlowMasterId = flowMasterAdapterModel.Id,
                MyUserId = myUserAdapterModel.Id,
                Approve = approve,
            };
            await context.FlowHistory.AddAsync(history);
            await context.SaveChangesAsync();
        }
        #endregion

        #region 確認該使用者是否可以執行簽核流程
        public bool CheckCurrentActionUser(List<FlowUser> flowUsers,
            MyUserAdapterModel myUserAdapterModel, FlowMasterAdapterModel flowMasterAdapterModel)
        {
            var flowUserCurrent = flowUsers.Where(x => x.Level == flowMasterAdapterModel.ProcessLevel
            && x.OnlyCC == false && x.Completion == false).ToList();
            if (flowUserCurrent.Count() == 0)
            {
                return false; ;
            }
            else
            {
                var findUser = flowUserCurrent.FirstOrDefault(x => x.MyUserId == myUserAdapterModel.Id);
                if (findUser == null)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        #region 送出
        public async Task SendAsync(FlowMasterAdapterModel flowMasterAdapterModel,
            ApproveOpinionModel approveOpinionModel)
        {
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);

            (var flowUsers, var user) = await GetUsersDataAsync(flowMasterAdapterModel);

            if (user.Id != flowMasterAdapterModel.MyUserId)
            {
                return;
            }

            var flowUser = flowUsers.FirstOrDefault(x => x.Level == 0);
            flowUser.Completion = true;
            flowMasterAdapterModel.ProcessLevel = 1;
            flowMasterAdapterModel.Status = 1;

            CopyUserAutoCompletion(flowUsers, flowMasterAdapterModel.ProcessLevel);

            FindNextActionUser(flowUsers, flowMasterAdapterModel);

            await context.BulkUpdateAsync(flowUsers);
            await UpdateAsync(flowMasterAdapterModel);

            await AddHistoryRecord(user, flowMasterAdapterModel, $"使用者 {user.Name} 送出簽核申請", $"自動產生此紀錄", true);

            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
        }
        #endregion

        #region 退回申請者
        public async Task BackToSendAsync(FlowMasterAdapterModel flowMasterAdapterModel,
            ApproveOpinionModel approveOpinionModel)
        {
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);

            (var flowUsers, var user) = await GetUsersDataAsync(flowMasterAdapterModel);

            if (CheckCurrentActionUser(flowUsers, user, flowMasterAdapterModel) == false) return;

            foreach (var item in flowUsers)
            {
                item.Completion = false;
            }
            flowMasterAdapterModel.ProcessLevel = 0;
            flowMasterAdapterModel.Status = 0;

            FindNextActionUser(flowUsers, flowMasterAdapterModel);

            await context.BulkUpdateAsync(flowUsers);
            await UpdateAsync(flowMasterAdapterModel);

            await AddHistoryRecord(user, flowMasterAdapterModel,
                $"使用者 {user.Name} 退回申請者簽核", $"自動產生此紀錄", false);

            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
        }
        #endregion

        #region 同意
        public async Task AgreeAsync(FlowMasterAdapterModel flowMasterAdapterModel,
            ApproveOpinionModel approveOpinionModel)
        {
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);

            (var flowUsers, var user) = await GetUsersDataAsync(flowMasterAdapterModel);

            if (CheckCurrentActionUser(flowUsers, user, flowMasterAdapterModel) == false) return;

            var flowUserCurrentLevel = flowUsers.Where(x => x.Level == flowMasterAdapterModel.ProcessLevel).ToList();
            var currentUser = flowUserCurrentLevel.FirstOrDefault(x => x.MyUserId == user.Id);
            if (currentUser != null)
            {
                currentUser.Completion = true;
            }
            var remindUsers = flowUsers
                .Where(x => x.Level == flowMasterAdapterModel.ProcessLevel &&
                x.Completion == false).ToList();
            bool allProcessing = (remindUsers.Count() > 0) ? false : true;

            if (allProcessing)
            {
                #region 是否是最後一關關卡
                var lastLevel = flowUsers.OrderByDescending(x => x.Level)
                    .FirstOrDefault();
                if (flowMasterAdapterModel.ProcessLevel == lastLevel.Level)
                {
                    flowMasterAdapterModel.Status = 99;
                    flowMasterAdapterModel.ProcessLevel++;
                }
                else
                {
                    #region 還有關卡要繼續審核
                    flowMasterAdapterModel.Status = 1;
                    flowMasterAdapterModel.ProcessLevel++;

                    CopyUserAutoCompletion(flowUsers, flowMasterAdapterModel.ProcessLevel);
                    #endregion
                }
                #endregion
            }

            FindNextActionUser(flowUsers, flowMasterAdapterModel);

            await context.BulkUpdateAsync(flowUsers);
            await UpdateAsync(flowMasterAdapterModel);

            await AddHistoryRecord(user, flowMasterAdapterModel,
                $"使用者 {user.Name} 同意簽核申請", $"自動產生此紀錄", true);

            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
        }
        #endregion

        #region 退回
        public async Task DenyAsync(FlowMasterAdapterModel flowMasterAdapterModel,
            ApproveOpinionModel approveOpinionModel)
        {
            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);

            (var flowUsers, var user) = await GetUsersDataAsync(flowMasterAdapterModel);

            if (CheckCurrentActionUser(flowUsers, user, flowMasterAdapterModel) == false) return;

            RecoveryCompletion(flowUsers, flowMasterAdapterModel.ProcessLevel);

            flowMasterAdapterModel.ProcessLevel--;
            RecoveryCompletion(flowUsers, flowMasterAdapterModel.ProcessLevel);
            CopyUserAutoCompletion(flowUsers, flowMasterAdapterModel.ProcessLevel);

            if (flowMasterAdapterModel.ProcessLevel > 0)
            {
            }
            else
            {
                flowMasterAdapterModel.Status = 0;

            }

            FindNextActionUser(flowUsers, flowMasterAdapterModel);

            await context.BulkUpdateAsync(flowUsers);
            await UpdateAsync(flowMasterAdapterModel);

            await AddHistoryRecord(user, flowMasterAdapterModel,
                $"使用者 {user.Name} 退回簽核申請", $"自動產生此紀錄", false);

            CleanTrackingHelper.Clean<FlowMaster>(context);
            CleanTrackingHelper.Clean<FlowUser>(context);
            CleanTrackingHelper.Clean<FlowHistory>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
        }
        #endregion

        #endregion
    }
}
