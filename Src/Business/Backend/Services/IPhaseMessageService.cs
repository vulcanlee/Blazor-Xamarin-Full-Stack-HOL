using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPhaseMessageService
    {
        ILogger<PhaseMessageService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(PhaseMessageAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(PhaseMessageAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(PhaseMessageAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(PhaseMessageAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(PhaseMessageAdapterModel paraObject);
        Task EnableIt(PhaseMessageAdapterModel paraObject);
        Task<DataRequestResult<PhaseMessageAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<PhaseMessageAdapterModel> GetAsync(int id);
        Task<DataRequestResult<PhaseMessageAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(PhaseMessageAdapterModel paraObject);
    }
}