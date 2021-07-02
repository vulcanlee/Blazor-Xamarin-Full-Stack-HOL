using Backend.AdapterModels;
using CommonDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IOrderItemService
    {
        Task<VerifyRecordResult> AddAsync(OrderItemAdapterModel paraObject);
        Task<DataRequestResult<OrderItemAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<OrderItemAdapterModel> GetAsync(int id);
        Task<DataRequestResult<OrderItemAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(OrderItemAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<VerifyRecordResult> BeforeAddCheckAsync(OrderItemAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderItemAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderItemAdapterModel paraObject);
    }
}