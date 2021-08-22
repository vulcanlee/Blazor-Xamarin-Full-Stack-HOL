using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPhaseCategoryService
    {
        ILogger<PhaseCategoryService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(PhaseCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(PhaseCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(PhaseCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(PhaseCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(PhaseCategoryAdapterModel paraObject);
        Task EnableIt(PhaseCategoryAdapterModel paraObject);
        Task<DataRequestResult<PhaseCategoryAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<PhaseCategoryAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(PhaseCategoryAdapterModel paraObject);
    }
}