using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBusiness.Helpers;

namespace Backend.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            string returnUrl = Url.Content("~/");
            try
            {
                // �M���w�g�s�b���n�J Cookie ���e
                await HttpContext.SignOutAsync(MagicHelper.CookieAuthenticationScheme);
            }
            catch
            {
            }
            return LocalRedirect(Url.Content("~/Login"));
        }
    }
}
