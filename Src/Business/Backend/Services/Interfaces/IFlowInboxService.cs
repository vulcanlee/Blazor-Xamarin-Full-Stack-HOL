using AutoMapper;
using Backend.AdapterModels;
using Backend.Helpers;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowInboxService
    {
        UserHelper CurrentUserHelper { get; }
        ILogger<FlowInboxService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(FlowInboxAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowInboxAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowInboxAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowInboxAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<FlowInboxAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<FlowInboxAdapterModel> GetAsync(int id);
        Task MailReadedAsync(FlowInboxAdapterModel flowInboxAdapterModel);
        Task<VerifyRecordResult> UpdateAsync(FlowInboxAdapterModel paraObject);
    }
}