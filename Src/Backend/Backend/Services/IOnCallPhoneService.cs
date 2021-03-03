using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IOnCallPhoneService
    {
        Task<VerifyRecordResult> AddAsync(OnCallPhoneAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(OnCallPhoneAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(OnCallPhoneAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(OnCallPhoneAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<OnCallPhoneAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<OnCallPhoneAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(OnCallPhoneAdapterModel paraObject);
    }
}