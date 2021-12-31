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

        public async Task<string> CheckWetherCanChangePassword(MyUserAdapterModel myUserAdapterModel, string newPassword)
        {
            string result = "";
            CleanTrackingHelper.Clean<AccountPolicy>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
            AccountPolicy AccountPolicy = await context.AccountPolicy
                .OrderBy(x=>x.Id)
                .FirstOrDefaultAsync();
            string encodePassword = PasswordHelper.GetPasswordSHA(myUserAdapterModel.Salt, newPassword);
            if (encodePassword == myUserAdapterModel.Password)
            {
                result = "不可以變更成為現在正在使用的密碼";
            }
            else
            {
                if (AccountPolicy.EnablePasswordHistory)
                {
                    var history = await context.MyUserPasswordHistory
                        .FirstOrDefaultAsync(x => x.MyUserId == myUserAdapterModel.Id &&
                        x.Password == encodePassword);
                    if (history != null)
                    {
                        result = "不可以變更成為之前用過的密碼";
                    }
                }
            }
            return result;
        }
        public async Task ChangePassword(MyUserAdapterModel myUserAdapterModel, string newPassword,
            string ip)
        {
            CleanTrackingHelper.Clean<AccountPolicy>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
            AccountPolicy AccountPolicy = await context.AccountPolicy
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
            string encodePassword =
                PasswordHelper.GetPasswordSHA(myUserAdapterModel.Salt, newPassword);
            myUserAdapterModel.Password = encodePassword;
            var myUser = Mapper.Map<MyUser>(myUserAdapterModel);
            CleanTrackingHelper.Clean<MyUser>(context);

            #region 更新下次要變更密碼的時間
            if (AccountPolicy.EnableCheckPasswordAge)
            {
                myUser.ForceChangePasswordDatetime = DateTime.Now
                    .AddDays(AccountPolicy.PasswordAge);
            }
            myUser.ForceChangePassword = false;
            #endregion

            context.Entry(myUser).State = EntityState.Modified;
            await context.SaveChangesAsync();

            if (AccountPolicy.EnablePasswordHistory == true)
            {
                MyUserPasswordHistory myUserPasswordHistory = new MyUserPasswordHistory()
                {
                    MyUserId = myUser.Id,
                    IP = ip,
                    Password = myUser.Password,
                    ChangePasswordDatetime = DateTime.Now,
                };

                await context.AddAsync(myUserPasswordHistory);
                await context.SaveChangesAsync();

                while(true)
                {
                    #region 只會記錄下系統指定的變更密碼數量 AccountPolicy.PasswordHistory
                    var myUserPasswordHistories = await context.MyUserPasswordHistory
                        .Where(x => x.MyUserId == myUser.Id)
                        .OrderBy(x => x.ChangePasswordDatetime)
                        .ToListAsync();
                    if(myUserPasswordHistories.Count > AccountPolicy.PasswordHistory)
                    {
                        var first = myUserPasswordHistories.First();
                        context.Remove(first);
                        await context.SaveChangesAsync();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                    #endregion
                }
            }
            CleanTrackingHelper.Clean<AccountPolicy>(context);
            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(context);
        }
    }
}
