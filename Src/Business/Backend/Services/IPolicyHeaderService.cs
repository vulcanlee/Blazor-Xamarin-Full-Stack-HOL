using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPolicyHeaderService
    {
        ILogger<PolicyHeaderService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(PolicyHeaderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(PolicyHeaderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(PolicyHeaderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(PolicyHeaderAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(PolicyHeaderAdapterModel paraObject);
        Task EnableIt(PolicyHeaderAdapterModel paraObject);
        Task<DataRequestResult<PolicyHeaderAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<PolicyHeaderAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(PolicyHeaderAdapterModel paraObject);
    }
}