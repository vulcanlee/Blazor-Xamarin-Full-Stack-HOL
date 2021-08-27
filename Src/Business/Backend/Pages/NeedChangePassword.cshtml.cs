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

        public NeedChangePasswordModel(IMyUserService myUserService, ILogger<NeedChangePasswordModel> logger,
            SystemLogHelper systemLogHelper, IHttpContextAccessor httpContextAccessor)
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
        }
        [BindProperty]
        public string NewPassword { get; set; } = "";

        [BindProperty]
        public string AgainPassword { get; set; } = "";
        public string PasswordType { get; set; } = "password";
        public string Msg { get; set; }
        public string ReturnUrl { get; set; }
        public SystemLogHelper SystemLogHelper { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public async Task OnGetAsync()
        {
            ClaimsPrincipal claimsPrincipal = HttpContextAccessor.HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated == false)
            {
                Msg = "使用者尚未登入，請先進行帳號密碼身分驗證程序";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            ClaimsPrincipal claimsPrincipal = HttpContextAccessor.HttpContext.User;
             if (claimsPrincipal.Identity.IsAuthenticated == false)
            {
                Msg = "無法變更密碼，請先進行帳號密碼身分驗證程序";
                return Page();
            }
          else if (NewPassword != AgainPassword)
            {
                Msg = "請確認兩次輸入的密碼都是相同的";
                return Page();
            }
            else
            {
                var userId = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.Sid)?.Value);
                var myUser = await myUserService.GetAsync(userId);

                if (myUser.Status == false)
                {
                    #region 使用者已經被停用，無法變更密碼
                    Msg = $"使用者 {myUser.Account} 已經被停用，無法變更密碼";
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

                myUser.Password =
                    PasswordHelper.GetPasswordSHA(myUser.Salt, NewPassword);
                myUser.ForceChangePassword = false;
                await myUserService.UpdateAsync(myUser);
                Msg = $"使用者 {myUser.Account} / {myUser.Name} " +
                    $"已經變更密碼 {DateTime.Now}";
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
