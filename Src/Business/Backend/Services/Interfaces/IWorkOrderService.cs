using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IWorkOrderService
    {
        ILogger<WorkOrderService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(WorkOrderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(WorkOrderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkOrderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkOrderAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<WorkOrderAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<WorkOrderAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(WorkOrderAdapterModel paraObject);
    }
}