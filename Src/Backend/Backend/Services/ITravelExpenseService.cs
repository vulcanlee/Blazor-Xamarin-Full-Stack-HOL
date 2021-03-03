using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ITravelExpenseService
    {
        Task<VerifyRecordResult> AddAsync(TravelExpenseAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(TravelExpenseAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(TravelExpenseAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(TravelExpenseAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<TravelExpenseAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<TravelExpenseAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(TravelExpenseAdapterModel paraObject);
    }
}