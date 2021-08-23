using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowMasterService
    {
        ILogger<FlowMasterService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowMasterAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<FlowMasterAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<FlowMasterAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(FlowMasterAdapterModel paraObject);
    }
}