using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IWorkingLogService
    {
        Task<VerifyRecordResult> AddAsync(WorkingLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(WorkingLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkingLogAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkingLogAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<WorkingLogAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<WorkingLogAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(WorkingLogAdapterModel paraObject);
    }
}