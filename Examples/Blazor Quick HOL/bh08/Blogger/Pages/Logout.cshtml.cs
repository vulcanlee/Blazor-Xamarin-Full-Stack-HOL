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
                // 清除已經存在的登入 Cookie 內容
                await HttpContext.SignOutAsync(MagicHelper.CookieAuthenticationScheme);
            }
            catch
            {
            }
            return LocalRedirect(Url.Content("~/Login"));
        }
    }
}
