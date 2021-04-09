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
            Username = "user1";
            Password = "pw";
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
                // �M���w�g�s�b���n�J Cookie ���e
                await HttpContext
                    .SignOutAsync(
                    MagicHelper.CookieAuthenticationScheme);
                logger.LogInformation("�ϥΪ̵n�X");
            }
            catch { }
        }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            (MyUserAdapterModel user, string message) = await myUserService.CheckUser(Username, Password);

            if (user == null)
            {
                Msg = message;
                logger.LogInformation($"�ϥΪ� ({Username} / {Password}) �n�J����");
            }
            else
            {
                string returnUrl = Url.Content("~/");

                #region �[�J�o�ӨϥΪ̻ݭn�Ψ쪺 �ŧi���� Claim Type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, user.Account),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                };
                if (user.IsManager == true)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                }
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
                    string error = ex.Message;
                }
                #endregion

                logger.LogInformation($"�ϥΪ� ({Username}) �n�J���\");
                return LocalRedirect(returnUrl);
            }
            return Page();
        }
    }
}
