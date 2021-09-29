using System.Threading.Tasks;
using Blogger.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogger.Pages
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
