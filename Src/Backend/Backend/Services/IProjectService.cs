using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IProjectService
    {
        Task<VerifyRecordResult> AddAsync(ProjectAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(ProjectAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(ProjectAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(ProjectAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<ProjectAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<ProjectAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(ProjectAdapterModel paraObject);
    }
}