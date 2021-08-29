using AutoMapper;
using Backend.AdapterModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IChangePasswordService
    {
        AuthenticationStateProvider AuthenticationStateProvider { get; }
        IMapper Mapper { get; }

        Task ChangePassword(MyUserAdapterModel myUserAdapterModel, string newPassword, string ip);
        Task<string> CheckWetherCanChangePassword(MyUserAdapterModel myUserAdapterModel, string newPassword);
        Task<MyUserAdapterModel> GetCurrentUser();
    }
}