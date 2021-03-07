using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMyUserService
    {
        Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MyUserAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject);
        Task<(MyUserAdapterModel, string)> CheckUser(string account, string password);
    }
}