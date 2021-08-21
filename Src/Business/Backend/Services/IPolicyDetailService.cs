using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPolicyDetailService
    {
        ILogger<PolicyDetailService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(PolicyDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(PolicyDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(PolicyDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(PolicyDetailAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(PolicyDetailAdapterModel paraObject);
        Task EnableIt(PolicyDetailAdapterModel paraObject);
        Task<DataRequestResult<PolicyDetailAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<PolicyDetailAdapterModel> GetAsync(int id);
        Task<DataRequestResult<PolicyDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(PolicyDetailAdapterModel paraObject);
    }
}