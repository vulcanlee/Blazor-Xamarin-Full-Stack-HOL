using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ILeaveFormService
    {
        Task<VerifyRecordResult> AddAsync(LeaveFormAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(LeaveFormAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(LeaveFormAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(LeaveFormAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<LeaveFormAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<LeaveFormAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(LeaveFormAdapterModel paraObject);
    }
}