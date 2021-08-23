using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IAuditHistoryService
    {
        ILogger<AuditHistoryService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(AuditHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(AuditHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(AuditHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(AuditHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<AuditHistoryAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<AuditHistoryAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(AuditHistoryAdapterModel paraObject);
    }
}