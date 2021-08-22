using AutoMapper;
using Backend.AdapterModels;
using CommonDomain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IMenuRoleService
    {
        IMapper Mapper { get; }

        Task<VerifyRecordResult> AddAsync(MenuRoleAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeAddCheckAsync(MenuRoleAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeDeleteCheckAsync(MenuRoleAdapterModel paraObject);
        Task<VerifyRecordResult> BeforeUpdateCheckAsync(MenuRoleAdapterModel paraObject);
        Task<VerifyRecordResult> DeleteAsync(int id);
        Task<DataRequestResult<MenuRoleAdapterModel>> GetAsync(DataRequest dataRequest);
        Task<MenuRoleAdapterModel> GetAsync(int id);
        Task<MenuRoleAdapterModel> GetAsync(string name);
        Task<List<MenuRoleAdapterModel>> GetForUserAsync();
        Task<VerifyRecordResult> UpdateAsync(MenuRoleAdapterModel paraObject);
    }
}