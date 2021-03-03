using Backend.AdapterModels;
using ShareDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IExceptionRecordService
    {
        Task<VerifyRecordResult> AddAsync(ExceptionRecordAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(ExceptionRecordAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(ExceptionRecordAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(ExceptionRecordAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<ExceptionRecordAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<ExceptionRecordAdapterModel> GetAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(ExceptionRecordAdapterModel paraObject);
    }
}