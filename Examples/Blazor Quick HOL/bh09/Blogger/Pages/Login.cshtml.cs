using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blogger.Helpers;
using Blogger.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogger.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public LoginModel(IMyUserAuthenticationService myUserAuthenticationService)
        {
            MyUserAuthenticationService = myUserAuthenticationService;
#if DEBUG
            Username = "user1";
            Password = "pw";
            PasswordType = "";
#endif
        }
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";
        public string PasswordType { get; set; } = "password";
        public string Msg { get; set; }
        public string ReturnUrl { get; set; }
        public IMyUserAuthenticationService MyUserAuthenticationService { get; }

        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool checkResult = await MyUserAuthenticationService.CheckUserAsync(Username, Password);
            if (checkResult == false)
            {
                Msg = "帳號或者密碼不可為空白";
            }
            else
            {
                string returnUrl = Url.Content("~/");

                #region 加入這個使用者需要用到的 宣告類型 Claim Type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Name, Username),
                };
                #endregion

                #region 建立 宣告式身分識別
                // ClaimsIdentity類別是宣告式身分識別的具體執行, 也就是宣告集合所描述的身分識別
                var claimsIdentity = new ClaimsIdentity(
                    claims, MagicHelper.CookieAuthenticationScheme);
                #endregion

                #region 建立關於認證階段需要儲存的狀態
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };
                #endregion

                #region 進行使用登入
                try
                {
                    await HttpContext.SignInAsync(
                    MagicHelper.CookieAuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
                #endregion

                Msg = $"使用者 ({Username}) 登入成功";
                return LocalRedirect(returnUrl);
            }
            return Page();
        }
    }
}
