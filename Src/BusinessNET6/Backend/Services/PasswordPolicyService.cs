using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Domains.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using BAL.Factories;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using CommonDomain.Enums;
    using System;
    using System.Threading;

    public class PasswordPolicyService : IPasswordPolicyService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<PasswordPolicyService> Logger { get; }
        #endregion

        #region 建構式
        public PasswordPolicyService(BackendDBContext context, IMapper mapper,
            ILogger<PasswordPolicyService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region 檢查密碼政策
        public async Task CheckPasswordAge(CancellationToken cancellationToken)
        {
            CleanTrackingHelper.Clean<AccountPolicy>(context);
            CleanTrackingHelper.Clean<MyUser>(context);

            AccountPolicy AccountPolicy = await context.AccountPolicy
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
            cancellationToken.ThrowIfCancellationRequested();
            List<MyUser> myUsers = await context.MyUser
                .ToListAsync();
            cancellationToken.ThrowIfCancellationRequested();

            var enableCheckPasswordAge = AccountPolicy.EnableCheckPasswordAge;
            var passwordAge = AccountPolicy.PasswordAge;

            if (enableCheckPasswordAge == true)
            {
                foreach (var item in myUsers)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (DateTime.Now > item.ForceChangePasswordDatetime)
                    {
                        #region 該使用者已經達到要變更密碼的時間
                        item.ForceChangePasswordDatetime = DateTime.Now.AddDays(passwordAge);
                        item.ForceChangePassword = true;
                        context.Update(item);
                        await context.SaveChangesAsync();
                        #endregion
                    }
                }
            }

            CleanTrackingHelper.Clean<MyUser>(context);
            CleanTrackingHelper.Clean<AccountPolicy>(context);

        }
        #endregion
    }
}
