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
                // �M���w�g�s�b���n�J Cookie ���e
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
                #region �������ҥ��ѡA�ϥΪ̤��s�b
                Msg = $"�������ҥ��ѡA�ϥΪ�({Username}���s�b : {message})";
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
                    logger.LogInformation("�ϥΪ̵n�X");
                });
                return Page();
                #endregion
            }

            if (user.Status == false)
            {
                #region �ϥΪ̤w�g�Q���ΡA�L�k�n�J
                Msg = $"�ϥΪ� {user.Account} �w�g�Q���ΡA�L�k�n�J";
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

            #region �[�J�o�ӨϥΪ̻ݭn�Ψ쪺 �ŧi���� Claim Type
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, user.Account),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(MagicHelper.MenuRoleClaim, user.MenuRoleId.ToString()),
                    new Claim(MagicHelper.MenuRoleNameClaim, user.MenuRole?.Name),
                };

            #region �Y�� �}�o�H���A�[�J �}�o�H�� �M�ݪ�����
            if (MagicHelper.�}�o�̱b�� == Username.ToLower())
            {
                claims.Add(new Claim(ClaimTypes.Role, MagicHelper.�}�o�̪������n��));
            }
            #endregion

            #region �[�J�ӨϥΪ̻ݭn�[�J����������
            var menuDatas = user.MenuRole.MenuData
                 .Where(x => x.Enable == true).ToList();

            foreach (var item in menuDatas)
            {
                if (item.IsGroup == false)
                {
                    if (item.CodeName.ToLower() != MagicHelper.�}�o�̪������n��)
                    {
                        // �קK�ϥΪ̦ۤv�[�J�@�� �}�o�H���M�� ������
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

            #region �إ� �ŧi�������ѧO
            // ClaimsIdentity���O�O�ŧi�������ѧO���������, �]�N�O�ŧi���X�Ҵy�z�������ѧO
            var claimsIdentity = new ClaimsIdentity(
                claims, MagicHelper.CookieAuthenticationScheme);
            #endregion

            #region �إ�����{�Ҷ��q�ݭn�x�s�����A
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };
            #endregion

            #region �i��ϥεn�J
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

            Msg = $"�ϥΪ� ({Username}) �n�J���\";
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
