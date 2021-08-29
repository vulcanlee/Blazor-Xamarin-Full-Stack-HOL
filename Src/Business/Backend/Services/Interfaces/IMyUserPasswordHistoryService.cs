using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMyUserPasswordHistoryService
    {
        ILogger<MyUserPasswordHistoryService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MyUserPasswordHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserPasswordHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserPasswordHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserPasswordHistoryAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<MyUserPasswordHistoryAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MyUserPasswordHistoryAdapterModel> GetAsync(int id);
        Task<DataRequestResult<MyUserPasswordHistoryAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(MyUserPasswordHistoryAdapterModel paraObject);
    }
}