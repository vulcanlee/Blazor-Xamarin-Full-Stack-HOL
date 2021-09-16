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
                Msg = "�b���Ϊ̱K�X���i���ť�";
            }
            else
            {
                string returnUrl = Url.Content("~/");

                #region �[�J�o�ӨϥΪ̻ݭn�Ψ쪺 �ŧi���� Claim Type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Name, Username),
                };
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
                return LocalRedirect(returnUrl);
            }
            return Page();
        }
    }
}
