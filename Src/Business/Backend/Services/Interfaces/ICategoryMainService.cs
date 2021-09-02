using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ICategoryMainService
    {
        ILogger<CategoryMainService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(CategoryMainAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(CategoryMainAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(CategoryMainAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(CategoryMainAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(CategoryMainAdapterModel paraObject);
        Task EnableIt(CategoryMainAdapterModel paraObject);
        Task<List<CategoryMainAdapterModel>> GetAsync();
        Task<DataRequestResult<CategoryMainAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<CategoryMainAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(CategoryMainAdapterModel paraObject);
    }
}