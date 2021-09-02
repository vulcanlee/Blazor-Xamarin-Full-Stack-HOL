using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ICategorySubService
    {
        ILogger<CategorySubService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(CategorySubAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(CategorySubAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(CategorySubAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(CategorySubAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(CategorySubAdapterModel paraObject);
        Task EnableIt(CategorySubAdapterModel paraObject);
        Task<DataRequestResult<CategorySubAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<CategorySubAdapterModel> GetAsync(int id);
        Task<DataRequestResult<CategorySubAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task<VerifyRecordResult> UpdateAsync(CategorySubAdapterModel paraObject);
    }
}