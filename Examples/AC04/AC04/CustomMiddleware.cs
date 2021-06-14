using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AC04
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            // 呼叫下一個中介軟體要執行的內容，在這裡不要寫入任何內容到 HTTP Responet 回應內
            Console.WriteLine($"呼叫第 3 個客製化中介軟體");

            await _next(httpContext);

            // 自這裡進行寫入日誌或其他工作，在這裡不要寫入任何內容到 HTTP Responet 回應內
            Console.WriteLine($"結束執行第 3 個客製化中介軟體");
            Console.WriteLine();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
