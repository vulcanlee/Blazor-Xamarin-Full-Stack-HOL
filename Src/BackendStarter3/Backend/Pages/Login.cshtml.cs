using Backend.AdapterModels;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public LoginModel(IMyUserService myUserService, ILogger<LoginModel> logger,
            SystemLogHelper systemLogHelper, IHttpContextAccessor httpContextAccessor)
        {
#if DEBUG
            Username = "god";
            Password = "123";
            PasswordType = "";
#endif
            this.myUserService = myUserService;
            this.logger = logger;
            SystemLogHelper = systemLogHelper;
            HttpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";
        [BindProperty]
        public string Version { get; set; } = "";
        public string PasswordType { get; set; } = "password";
        public string Msg { get; set; }
        public string ReturnUrl { get; set; }
        public SystemLogHelper SystemLogHelper { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

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
            }
            catch { }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            (MyUserAdapterModel user, string message) =
                await myUserService.CheckUser(Username, Password);

            if (user == null)
            {
                #region 身分驗證失敗，使用者不存在
                Msg = $"身分驗證失敗，使用者({Username}不存在 : {message})";
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.User,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                }, () =>
                {
                    logger.LogInformation("使用者登出");
                });
                return Page();
                #endregion
            }

            if (user.Status == false)
            {
                #region 使用者已經被停用，無法登入
                Msg = $"使用者 {user.Account} 已經被停用，無法登入";
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.User,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                }, () =>
                {
                    logger.LogInformation($"{Msg}");
                });
                return Page();
                #endregion
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
                    if (item.CodeName.ToLower() != MagicHelper.開發者的角色聲明)
                    {
                        // 避免使用者自己加入一個 開發人員專屬 的角色
                        if (!((item.CodeName.Contains("/") == true ||
                            item.CodeName.ToLower().Contains("http") == true)))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item.CodeName));
                        }
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

            Msg = $"使用者 ({Username}) 登入成功";
            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
            {
                Message = Msg,
                Category = LogCategories.User,
                Content = "",
                LogLevel = LogLevels.Information,
                Updatetime = DateTime.Now,
                IP = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
            }, () =>
            {
                logger.LogInformation($"{Msg}");
            });
            return LocalRedirect(returnUrl);
            return Page();
        }
    }
}
