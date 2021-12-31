using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;

namespace Backend.Services
{
    public interface IProductService
    {
        ILogger<ProductService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(ProductAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<ProductAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<ProductAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(ProductAdapterModel paraObject);
    }
}