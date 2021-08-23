using AutoMapper;
using Backend.AdapterModels;
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
    public class CurrentUserHelper
    {
        public CurrentUserHelper(AuthenticationStateProvider authenticationStateProvider,
            IMapper mapper, IMyUserService myUserService)
        {
            AuthenticationStateProvider = authenticationStateProvider;
            Mapper = mapper;
            MyUserService = myUserService;
        }

        public AuthenticationStateProvider AuthenticationStateProvider { get; }
        public IMapper Mapper { get; }
        public IMyUserService MyUserService { get; }

        public async Task<MyUserAdapterModel> GetCurrentUserAsync()
        {
            #region  取得現在登入使用者資訊
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                var myUserId = Convert.ToInt32(user.FindFirst(c => c.Type == ClaimTypes.Sid)?.Value);
                var myUser = await MyUserService.GetAsync(myUserId);
                if (myUser.Id == 0) return null;
                var myUserAdapterModel = Mapper.Map<MyUserAdapterModel>(myUser);
                return myUserAdapterModel;
            }
            else
            {
                return null;
            }
            #endregion

        }
    }
}
