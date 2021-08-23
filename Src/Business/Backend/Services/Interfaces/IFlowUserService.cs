using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IFlowUserService
    {
        ILogger<FlowUserService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(FlowUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(FlowUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(FlowUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(FlowUserAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(FlowUserAdapterModel paraObject);
        Task EnableIt(FlowUserAdapterModel paraObject);
        Task<DataRequestResult<FlowUserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<FlowUserAdapterModel> GetAsync(int id);
        Task<DataRequestResult<FlowUserAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(FlowUserAdapterModel paraObject);
    }
}