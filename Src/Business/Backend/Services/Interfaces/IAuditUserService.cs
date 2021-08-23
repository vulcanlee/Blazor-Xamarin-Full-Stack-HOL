using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IAuditUserService
    {
        ILogger<AuditUserService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(AuditUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(AuditUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(AuditUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(AuditUserAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(AuditUserAdapterModel paraObject);
        Task EnableIt(AuditUserAdapterModel paraObject);
        Task<DataRequestResult<AuditUserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<AuditUserAdapterModel> GetAsync(int id);
        Task<DataRequestResult<AuditUserAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(AuditUserAdapterModel paraObject);
    }
}