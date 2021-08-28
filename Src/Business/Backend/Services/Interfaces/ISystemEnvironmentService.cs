using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ISystemEnvironmentService
    {
        ILogger<SystemEnvironmentService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(SystemEnvironmentAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(SystemEnvironmentAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(SystemEnvironmentAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(SystemEnvironmentAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<SystemEnvironmentAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<SystemEnvironmentAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(SystemEnvironmentAdapterModel paraObject);
    }
}