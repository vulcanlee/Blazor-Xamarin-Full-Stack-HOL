using Backend.AdapterModels;
using Entities.Models;
using ShareDomain.DataModels;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IOrderService
    {
        Task<VerifyRecordResult> AddAsync(OrderAdapterModel paraObject);
        Task<DataRequestResult<OrderAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<OrderAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(OrderAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<VerifyRecordResult> BeforeAddCheckAsync(OrderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(OrderAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(OrderAdapterModel paraObject);
    }
}