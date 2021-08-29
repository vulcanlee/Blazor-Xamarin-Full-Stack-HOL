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
            ISystemEnvironmentService systemEnvironmentService, IChangePasswordService changePasswordService)
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
            SystemEnvironmentService = systemEnvironmentService;
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
        public ISystemEnvironmentService SystemEnvironmentService { get; }
        SystemEnvironmentAdapterModel systemEnvironmentAdapterModel = null;
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
            if (systemEnvironmentAdapterModel == null)
            {
                systemEnvironmentAdapterModel =
                    await SystemEnvironmentService.GetAsync();
            }
            PasswordStrength passwordStrength = (PasswordStrength)systemEnvironmentAdapterModel.PasswordComplexity;
            PasswordHint = PasswordCheck.PasswordHint(passwordStrength);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await GetPasswordHint();
            PasswordStrength passwordStrength = (PasswordStrength)systemEnvironmentAdapterModel.PasswordComplexity;

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
                        IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    });
                    logger.LogInformation($"{Msg}");
                    return Page();
                    #endregion
                }

                await changePasswordService.ChangePassword(myUser, NewPassword,
                    HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());

                Msg = $"�ϥΪ� {myUser.Account} / {myUser.Name} " +
                    $"�w�g�ܧ�K�X {DateTime.Now}";
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.User,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                });
                logger.LogInformation($"{Msg}");
            }

            string returnUrl = Url.Content("~/");
            return LocalRedirect(returnUrl);
        }
    }
}
