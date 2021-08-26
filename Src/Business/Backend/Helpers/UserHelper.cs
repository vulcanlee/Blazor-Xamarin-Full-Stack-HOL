using AutoMapper;
using Backend.AdapterModels;
using Backend.Models;
using Backend.Services;
using Domains.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class UserHelper
    {
        public UserHelper(AuthenticationStateProvider authenticationStateProvider,
            IMapper mapper, IMyUserService myUserService)
        {
            AuthenticationStateProvider = authenticationStateProvider;
            Mapper = mapper;
            MyUserService = myUserService;
        }

        public int CustomUserId { get; set; } = 0;
        public string CustomUserName { get; set; } = "";
        public AuthenticationStateProvider AuthenticationStateProvider { get; }
        public IMapper Mapper { get; }
        public IMyUserService MyUserService { get; }
        public MyUserAdapterModel CurrentMyUserAdapterModel { get; set; }
        public async Task<MyUserAdapterModel> GetCurrentUserAsync()
        {
            #region  取得現在登入使用者資訊
            if (CustomUserId != 0)
            {
                var myUser = await MyUserService.GetAsync(CustomUserId);
                CurrentMyUserAdapterModel = myUser;
                return myUser;
            }
            else
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    var myUserId = Convert.ToInt32(user.FindFirst(c => c.Type == ClaimTypes.Sid)?.Value);
                    var myUser = await MyUserService.GetAsync(myUserId);
                    if (myUser.Id == 0) return null;
                    var myUserAdapterModel = Mapper.Map<MyUserAdapterModel>(myUser);
                    CurrentMyUserAdapterModel = myUserAdapterModel;

                    return myUserAdapterModel;
                }
                else
                {
                    CurrentMyUserAdapterModel = null;
                    return null;
                }
            }
            #endregion

        }
        public async Task<MyUserAdapterModel> GetCurrentUserByShowFlowActionAsync(CurrentUser currentUser)
        {
            #region  取得現在登入使用者資訊
            await Task.Yield();
            if (CustomUserId != 0)
            {
                //var myUser = await MyUserService.GetAsync(CustomUserId);
                //return myUser;
                return currentUser.SimulatorMyUserAdapterModel;
            }
            else
            {
                return currentUser.LoginMyUserAdapterModel;
            }
            #endregion
        }
    }
}
