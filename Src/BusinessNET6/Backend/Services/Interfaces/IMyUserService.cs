using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMyUserService
    {
        IConfiguration Configuration { get; }
        ILogger<MyUserService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MyUserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MyUserAdapterModel paraObject);
        Task<(MyUserAdapterModel, string)> CheckUser(string account, string password);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(MyUserAdapterModel paraObject);
        Task EnableIt(MyUserAdapterModel paraObject);
        Task<DataRequestResult<MyUserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MyUserAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(MyUserAdapterModel paraObject);
        Task<MyUserAdapterModel> UserByAccount(string account);
    }
}