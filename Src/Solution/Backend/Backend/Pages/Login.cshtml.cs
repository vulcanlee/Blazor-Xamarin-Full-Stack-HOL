using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.AdapterModels;
using Backend.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBusiness.Helpers;

namespace Backend.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IHoluserService holuserService;

        public LoginModel(IHoluserService holuserService)
        {
#if DEBUG
            Username = "user1";
            Password = "pw";
            PasswordType = "";
            this.holuserService = holuserService;
#endif
        }
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";
        public string PasswordType { get; set; } = "password";
        public string Msg { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                // 清除已經存在的登入 Cookie 內容
                await HttpContext
                    .SignOutAsync(
                    MagicHelper.CookieAuthenticationScheme);
            }
            catch { }
        }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            (HoluserAdapterModel user, string message) = await holuserService.CheckUser(Username, Password);

            if (user == null)
            {
                Msg = message;
            }
            else
            {
                string returnUrl = Url.Content("~/");

                #region 加入這個使用者需要用到的 宣告類型 Claim Type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("TokenVersion", user.TokenVersion.ToString()),
                };
                if (user.Level == 4)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                }
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
                    string error = ex.Message;
                }
                #endregion

                return LocalRedirect(returnUrl);
            }
            return Page();
        }
    }
}
