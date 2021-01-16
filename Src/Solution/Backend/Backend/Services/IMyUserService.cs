using AutoMapper;
using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMyUserService
    {
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MyUserAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject);
    }
}