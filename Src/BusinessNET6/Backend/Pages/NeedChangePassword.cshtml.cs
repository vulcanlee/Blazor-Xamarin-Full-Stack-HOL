using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.AdapterModels;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using BAL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Backend.Pages
{
    [AllowAnonymous]
    public class NeedChangePasswordModel : PageModel
    {
        private readonly IMyUserService myUserService;
        private readonly ILogger<NeedChangePasswordModel> logger;
        private readonly IChangePasswordService changePasswordService;

        public NeedChangePasswordModel(IMyUserService myUserService, ILogger<NeedChangePasswordModel> logger,
            SystemLogHelper systemLogHelper, IHttpContextAccessor httpContextAccessor,
            IAccountPolicyService AccountPolicyService, IChangePasswordService changePasswordService)
        {
#if DEBUG
            NewPassword = "";
            AgainPassword = "";
            PasswordType = "";
#endif
            this.myUserService = myUserService;
            this.logger = logger;
            SystemLogHelper = systemLogHelper;
            HttpContextAccessor = httpContextAccessor;
            AccountPolicyService = AccountPolicyService;
            this.changePasswordService = changePasswordService;
        }
        [BindProperty]
        public string NewPassword { get; set; } = "";

        [BindProperty]
        public string AgainPassword { get; set; } = "";

        [BindProperty]
        public string PasswordType { get; set; } = "password";
        [BindProperty]
        public string PasswordHint { get; set; } = "";

        [BindProperty]
        public string Msg { get; set; }
        public string ReturnUrl { get; set; }
        public SystemLogHelper SystemLogHelper { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IAccountPolicyService AccountPolicyService { get; }
        AccountPolicyAdapterModel AccountPolicyAdapterModel = null;
        public async Task OnGetAsync()
        {
            await GetPasswordHint();
            ClaimsPrincipal claimsPrincipal = HttpContextAccessor.HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated == false)
            {
                Msg = "�ϥΪ̩|���n�J�A�Х��i��b���K�X�������ҵ{��";
            }
        }
        public async Task GetPasswordHint()
        {
            if (AccountPolicyAdapterModel == null)
            {
                AccountPolicyAdapterModel =
                    await AccountPolicyService.GetAsync();
            }
            PasswordStrength passwordStrength = (PasswordStrength)AccountPolicyAdapterModel.PasswordComplexity;
            PasswordHint = PasswordCheck.PasswordHint(passwordStrength);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await GetPasswordHint();
            PasswordStrength passwordStrength = (PasswordStrength)AccountPolicyAdapterModel.PasswordComplexity;

            ClaimsPrincipal claimsPrincipal = HttpContextAccessor.HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated == false)
            {
                Msg = "�L�k�ܧ�K�X�A�Х��i��b���K�X�������ҵ{��";
                return Page();
            }
            else if (NewPassword != AgainPassword)
            {
                Msg = "�нT�{�⦸��J���K�X���O�ۦP��";
                return Page();
            }
            else
            {
                var inputPasswordStrength = PasswordCheck.GetPasswordStrength(NewPassword);
                if(passwordStrength> inputPasswordStrength)
                {
                    Msg = "�K�X�j�פ����A�п�J�ŦX�K�X�F�����K�X";
                    return Page();
                }
                var userId = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.Sid)?.Value);
                var myUser = await myUserService.GetAsync(userId);

                if (myUser.Status == false)
                {
                    #region �ϥΪ̤w�g�Q���ΡA�L�k�ܧ�K�X
                    Msg = $"�ϥΪ� {myUser.Account} �w�g�Q���ΡA�L�k�ܧ�K�X";
                    await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                    {
                        Message = Msg,
                        Category = LogCategories.User,
                        Content = "",
                        LogLevel = LogLevels.Information,
                        Updatetime = DateTime.Now,
                        IP = HttpContextAccessor.GetConnectionIP(),
                    });
                    logger.LogInformation($"{Msg}");
                    return Page();
                    #endregion
                }
                Msg = await changePasswordService.CheckWetherCanChangePassword(myUser, NewPassword);
                if(string.IsNullOrEmpty(Msg) ==false)
                {
                    return Page();
                }
                await changePasswordService.ChangePassword(myUser, NewPassword,
                    HttpContextAccessor.GetConnectionIP());

                Msg = $"�ϥΪ� {myUser.Account} / {myUser.Name} " +
                    $"�w�g�ܧ�K�X {DateTime.Now}";
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.User,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
                logger.LogInformation($"{Msg}");
            }

            string returnUrl = Url.Content("~/");
            return LocalRedirect(returnUrl);
        }
    }
}
