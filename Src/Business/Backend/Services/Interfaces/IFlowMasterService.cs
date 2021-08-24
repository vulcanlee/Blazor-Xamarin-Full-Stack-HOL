using AutoMapper;
using Backend.AdapterModels;
using Backend.Helpers;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowMasterService
    {
        UserHelper UserHelper { get; }
        ILogger<FlowMasterService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(FlowMasterAdapterModel paraObject);
        Task AgreeAsync(FlowMasterAdapterModel flowMasterAdapterModel);
        Task BackToSendAsync(FlowMasterAdapterModel flowMasterAdapterModel);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DenyAsync(FlowMasterAdapterModel flowMasterAdapterModel);
        Task<DataRequestResult<FlowMasterAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<FlowMasterAdapterModel> GetAsync(int id);
        Task SendAsync(FlowMasterAdapterModel flowMasterAdapterModel);
        Task<VerifyRecordResult> UpdateAsync(FlowMasterAdapterModel paraObject);
    }
}