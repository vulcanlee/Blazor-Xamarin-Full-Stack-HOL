using Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Backend.Models;
using Backend.Helpers;

namespace Backend.Middlewares
{
    public class AdminSafeListMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminSafeListMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context, BlazorAppContext blazorAppContext)
        {
            var ip = context.GetRemoteIPAddress().ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = "";
            }
            else
            {
                //blazorAppContext.CurrentUserIP = ip;
            }
            await _next.Invoke(context);
        }
    }

    public static class AdminSafeListMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminSafeListMiddleware>();
        }
    }
}
