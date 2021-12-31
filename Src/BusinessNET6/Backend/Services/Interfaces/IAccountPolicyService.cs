using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IAccountPolicyService
    {
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(AccountPolicyAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(AccountPolicyAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(AccountPolicyAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(AccountPolicyAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<AccountPolicyAdapterModel> GetAsync();
        Task<DataRequestResult<AccountPolicyAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<AccountPolicyAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(AccountPolicyAdapterModel paraObject);
    }
}