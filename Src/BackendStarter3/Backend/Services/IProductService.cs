using Backend.AdapterModels;
using CommonDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IProductService
    {
        Task<VerifyRecordResult> AddAsync(ProductAdapterModel paraObject);
        Task<ProductAdapterModel> GetAsync(int id);
        Task<DataRequestResult<ProductAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<VerifyRecordResult> BeforeAddCheckAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(ProductAdapterModel paraObject);
    }
}