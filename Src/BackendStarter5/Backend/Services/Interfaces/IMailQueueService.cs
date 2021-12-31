using AutoMapper;
using Backend.AdapterModels;
using Backend.Models;
using CommonDomain.DataModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMailQueueService
    {
        ILogger<MailQueueService> Logger { get; }
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MailQueueAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MailQueueAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MailQueueAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MailQueueAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(long id);
        Task<DataRequestResult<MailQueueAdapterModel>> GetAsync(DataRequest dataRequest, MailQueueStatusCondition CurrentMailQueueStatusCondition);
        Task<MailQueueAdapterModel> GetAsync(long id);
        Task<List<MailQueueAdapterModel>> GetNotSentAsync();
        Task<VerifyRecordResult> UpdateAsync(MailQueueAdapterModel paraObject);
    }
}