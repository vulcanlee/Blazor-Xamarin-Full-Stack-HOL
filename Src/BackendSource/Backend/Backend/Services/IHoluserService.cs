using Backend.AdapterModels;
using Entities.Models;
using ShareDomain.DataModels;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IHoluserService
    {
        Task<VerifyRecordResult> AddAsync(HoluserAdapterModel paraObject);
        Task<DataRequestResult<HoluserAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<HoluserAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(HoluserAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<(HoluserAdapterModel, string)> CheckUser(string account, string password);
        Task<VerifyRecordResult> BeforeAddCheckAsync(HoluserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(HoluserAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(HoluserAdapterModel paraObject);
    }
}