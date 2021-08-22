using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IAuditMasterService
    {
        ILogger<AuditMasterService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(AuditMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(AuditMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(AuditMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(AuditMasterAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<AuditMasterAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<AuditMasterAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(AuditMasterAdapterModel paraObject);
    }
}