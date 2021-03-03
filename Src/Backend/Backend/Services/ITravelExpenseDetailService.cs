using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ITravelExpenseDetailService
    {
        Task<VerifyRecordResult> AddAsync(TravelExpenseDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(TravelExpenseDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(TravelExpenseDetailAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(TravelExpenseDetailAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<TravelExpenseDetailAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<TravelExpenseDetailAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(TravelExpenseDetailAdapterModel paraObject);
        Task<DataRequestResult<TravelExpenseDetailAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
    }
}