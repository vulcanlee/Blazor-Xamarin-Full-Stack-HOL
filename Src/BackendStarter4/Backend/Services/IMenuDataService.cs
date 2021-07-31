using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMenuDataService
    {
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MenuDataAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MenuDataAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MenuDataAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MenuDataAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task DisableIt(MenuDataAdapterModel paraObject);
        Task EnableIt(MenuDataAdapterModel paraObject);
        Task<DataRequestResult<MenuDataAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MenuDataAdapterModel> GetAsync(int id);
        Task<DataRequestResult<MenuDataAdapterModel>> GetByHeaderIDAsync(int id, DataRequest dataRequest);
        Task ReorderByHeaderIDAsync(int id);
        Task<VerifyRecordResult> UpdateAsync(MenuDataAdapterModel paraObject);
    }
}