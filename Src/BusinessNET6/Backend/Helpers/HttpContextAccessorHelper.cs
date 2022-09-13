using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class HttpContextAccessorHelper
    {
        public static string GetConnectionIP(this IHttpContextAccessor httpContextAccessor)
        {
            //var ip = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            var ip = httpContextAccessor?.HttpContext?.GetRemoteIPAddress().ToString();

            if (ip==null)
            {
                ip = "";
            }
            return ip;
        }
    }
}
