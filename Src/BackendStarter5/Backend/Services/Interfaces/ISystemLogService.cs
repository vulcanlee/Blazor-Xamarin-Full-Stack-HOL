using AutoMapper;
using Backend.AdapterModels;
using Microsoft.Extensions.Logging;
using CommonDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ISystemLogService
    {
        ILogger<SystemLogService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(SystemLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(SystemLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(SystemLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(SystemLogAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(long id);
        Task<DataRequestResult<SystemLogAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<SystemLogAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(SystemLogAdapterModel paraObject);
    }
}