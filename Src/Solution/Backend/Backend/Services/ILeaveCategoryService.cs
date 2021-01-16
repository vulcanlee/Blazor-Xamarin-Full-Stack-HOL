using AutoMapper;
using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ILeaveCategoryService
    {
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(LeaveCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(LeaveCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(LeaveCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(LeaveCategoryAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<LeaveCategoryAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<LeaveCategoryAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(LeaveCategoryAdapterModel paraObject);
    }
}