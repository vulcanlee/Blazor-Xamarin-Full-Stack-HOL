using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowHistoryService
    {
        ILogger<FlowHistoryService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(FlowHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<FlowHistoryAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<FlowHistoryAdapterModel> GetAsync(int id);
        Task<DataRequestResult<FlowHistoryAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(FlowHistoryAdapterModel paraObject);
    }
}