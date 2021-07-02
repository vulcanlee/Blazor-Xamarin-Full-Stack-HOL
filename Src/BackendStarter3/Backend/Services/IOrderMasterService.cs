using Backend.AdapterModels;
using CommonDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IOrderMasterService
    {
        Task<VerifyRecordResult> AddAsync(OrderMasterAdapterModel paraObject);
        Task<DataRequestResult<OrderMasterAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<OrderMasterAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(OrderMasterAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<VerifyRecordResult> BeforeAddCheckAsync(OrderMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderMasterAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderMasterAdapterModel paraObject);
    }
}