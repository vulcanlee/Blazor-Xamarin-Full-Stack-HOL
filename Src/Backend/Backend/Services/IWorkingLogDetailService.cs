using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IWorkingLogDetailService
    {
        Task<VerifyRecordResult> AddAsync(WorkingLogDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(WorkingLogDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(WorkingLogDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(WorkingLogDetailAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<WorkingLogDetailAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<WorkingLogDetailAdapterModel> GetAsync(int id);
        Task<DataRequestResult<WorkingLogDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(WorkingLogDetailAdapterModel paraObject);
    }
}