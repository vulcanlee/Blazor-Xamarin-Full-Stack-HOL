using Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
            blazorAppContext.CurrentUserIP = context.Connection.RemoteIpAddress.ToString();
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
