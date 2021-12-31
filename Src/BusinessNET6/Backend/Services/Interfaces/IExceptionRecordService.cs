using AutoMapper;
using CommonDomain.DataModels;
using Backend.AdapterModels;

namespace Backend.Services
{
    public interface IExceptionRecordService
    {
        IMapper Mapper { get; }

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