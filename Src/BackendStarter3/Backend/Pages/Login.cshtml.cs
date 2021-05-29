using Backend.AdapterModels;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShareBusiness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMyUserService myUserService;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(IMyUserService myUserService, ILogger<LoginModel> logger)
        {
#if DEBUG
            Username = "god";
            Password = "123";
            PasswordType = "";
#endif
            this.myUserService = myUserService;
            this.logger = logger;
        }
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";
        [BindProperty]
        public string Version { get; set; } = "";
        public string PasswordType { get; set; } = "password";
        public string Msg { get; set; }
        public async Task OnGetAsync()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            Version = version.ToString();
            try
            {
                // 清除已經存在的登入 Cookie 內容
                await HttpContext
                    .SignOutAsync(
                    MagicHelper.CookieAuthenticationScheme);
                logger.LogInformation("使用者登出");
            }
            catch { }
        }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            (MyUserAdapterModel user, string message) =
                await myUserService.CheckUser(Username, Password);

            if (user == null)
            {
                Msg = message;
                logger.LogInformation($"使用者 ({Username} / {Password}) 登入失敗");
                return Page();
            }

            if (user.Status == false)
            {
                Msg = $"使用者 {user.Account} 已經被停用";
                logger.LogInformation($"{Msg}");
                return Page();
            }

            string returnUrl = Url.Content("~/");

            #region 加入這個使用者需要用到的 宣告類型 Claim Type
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, user.Account),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(MagicHelper.MenuRoleClaim, user.MenuRoleId.ToString()),
                    new Claim(MagicHelper.MenuRoleNameClaim, user.MenuRole?.Name),
                };

            #region 若為 開發人員，加入 開發人員 專屬的角色
            if (MagicHelper.開發者帳號 == Username.ToLower())
            {
                claims.Add(new Claim(ClaimTypes.Role, MagicHelper.開發者的角色聲明));
            }
            #endregion

            #region 加入該使用者需要加入的相關角色
            var menuDatas = user.MenuRole.MenuData
                 .Where(x => x.Enable == true).ToList();

            foreach (var item in menuDatas)
            {
                if (item.IsGroup == false)
                {
                    // 避免使用者自己加入一個 開發人員專屬 的角色
                    if (item.CodeName != "/" &&
                        item.CodeName.ToLower() != MagicHelper.開發者的角色聲明)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.CodeName));
                    }
                }
            }
            #endregion
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

            logger.LogInformation($"使用者 ({Username}) 登入成功");
            return LocalRedirect(returnUrl);
            return Page();
        }
    }
}
