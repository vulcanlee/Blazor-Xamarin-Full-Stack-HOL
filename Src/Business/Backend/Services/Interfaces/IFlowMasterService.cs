using AutoMapper;
using Backend.AdapterModels;
using Backend.Helpers;
using Backend.Models;
using CommonDomain.DataModels;
using Domains.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowMasterService
    {
        ILogger<FlowMasterService> Logger { get; }
        IMapper Mapper { get; }
        UserHelper UserHelper { get; }

        Task<VerifyRecordResult> AddAsync(FlowMasterAdapterModel paraObject);
        Task AddHistoryRecord(MyUserAdapterModel myUserAdapterModel, FlowMasterAdapterModel flowMasterAdapterModel, string Summary, string Comment, bool approve);
        Task AddInboxRecord(FlowMasterAdapterModel paraObject, MyUserAdapterModel myUser, bool isCC);
        Task AgreeAsync(FlowMasterAdapterModel flowMasterAdapterModel, ApproveOpinionModel approveOpinionModel);
        Task BackToSendAsync(FlowMasterAdapterModel flowMasterAdapterModel, ApproveOpinionModel approveOpinionModel);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowMasterAdapterModel paraObject);
        bool CheckCurrentActionUser(List<FlowUser> flowUsers, MyUserAdapterModel myUserAdapterModel, FlowMasterAdapterModel flowMasterAdapterModel);
        Task<bool> CheckUserShowActionAsync(FlowMasterAdapterModel flowMasterAdapterModel, CurrentUser currentUser);
        void CopyUserAutoCompletion(List<FlowUser> flowUsers, int processLevel);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DenyAsync(FlowMasterAdapterModel flowMasterAdapterModel, ApproveOpinionModel approveOpinionModel);
        void FindNextActionUser(List<FlowUser> flowUsers, FlowMasterAdapterModel flowMasterAdapterModel);
        Task<DataRequestResult<FlowMasterAdapterModel>> GetAsync(DataRequest dataRequest, UserHelper UserHelper, CurrentUser CurrentUser);
        Task<FlowMasterAdapterModel> GetAsync(int id);
        Task<(List<FlowUser> flowUsers, MyUserAdapterModel user)> GetUsersDataAsync(FlowMasterAdapterModel flowMasterAdapterModel);
        Task<(List<FlowUser> flowUsers, MyUserAdapterModel user)> GetUsersDataByActionAsync(FlowMasterAdapterModel flowMasterAdapterModel, CurrentUser currentUser);
        Task NotifyInboxUsers(List<FlowUser> flowUsers, FlowMasterAdapterModel flowMasterAdapterModel, int level);
        void RecoveryCompletion(List<FlowUser> flowUsers, int processLevel);
        Task SendAsync(FlowMasterAdapterModel flowMasterAdapterModel, ApproveOpinionModel approveOpinionModel);
        Task<VerifyRecordResult> UpdateAsync(FlowMasterAdapterModel paraObject);
    }
}