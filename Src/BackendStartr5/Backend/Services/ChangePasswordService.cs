using AutoMapper;
using Backend.AdapterModels;
using Domains.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using BAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }
        public AuthenticationStateProvider AuthenticationStateProvider { get; }

        public ChangePasswordService(BackendDBContext context, IMapper mapper,
            AuthenticationStateProvider authenticationStateProvider)
        {
            this.context = context;
            Mapper = mapper;
            AuthenticationStateProvider = authenticationStateProvider;
        }

        public async Task<MyUserAdapterModel> GetCurrentUser()
        {
            #region  取得現在登入使用者資訊
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                var myUserId = Convert.ToInt32(user.FindFirst(c => c.Type == ClaimTypes.Sid)?.Value);
                var myUser = await context.MyUser
                    .FirstAsync(x => x.Id == myUserId);
                if (myUser == null) return null;
                var myUserAdapterModel = Mapper.Map<MyUserAdapterModel>(myUser);
                return myUserAdapterModel;
            }
            else
            {
                return null;
            }
            #endregion

        }

        public async Task ChangePassword(MyUserAdapterModel myUserAdapterModel, string newPassword,
            string ip)
        {
            string encodePassword =
                PasswordHelper.GetPasswordSHA(myUserAdapterModel.Salt, newPassword);
            myUserAdapterModel.Password = encodePassword;
            var myUser = Mapper.Map<MyUser>(myUserAdapterModel);
            CleanTrackingHelper.Clean<MyUser>(context);

            context.Entry(myUser).State = EntityState.Modified;
            await context.SaveChangesAsync();

            CleanTrackingHelper.Clean<MyUser>(context);
        }
    }
}
