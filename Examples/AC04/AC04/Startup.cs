using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AC04
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Case 1 自行客製中介軟體 1
            app.Use(async (context, next) =>
            {
                // 呼叫下一個中介軟體要執行的內容，在這裡不要寫入任何內容到 HTTP Responet 回應內
                Console.WriteLine($"呼叫第 1 個客製化中介軟體");
                Console.WriteLine($"路由路徑 {context.Request.Path}");

                await next.Invoke();

                // 自這裡進行寫入日誌或其他工作，在這裡不要寫入任何內容到 HTTP Responet 回應內
                Console.WriteLine($"結束執行第 1 個客製化中介軟體");
                Console.WriteLine($"回應狀態碼 {context.Response.StatusCode}");
                Console.WriteLine();
            });
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            #region Case 2 加入額外的提供靜態檔案的選項 StaticFileOptions 
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "MyImages")),
                RequestPath = "/Images"
            });
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            #region Case 3 自行客製中介軟體 2
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"呼叫第 2 個客製化中介軟體");
                Console.WriteLine($"路由路徑 {context.Request.Path}");

                // 限制這個 端點 僅能夠看到 "Hello World!" 文字
                if (context.Request.Path == "/Vulcan")
                {
                    await context.Response.WriteAsync("Hello World!");
                    return;
                }
                
                await next.Invoke();

                // 自這裡進行寫入日誌或其他工作，在這裡不要寫入任何內容到 HTTP Responet 回應內
                Console.WriteLine($"結束執行第 2 個客製化中介軟體");
            });
            #endregion

            #region Case 4 自行客製中介軟體 3
            app.UseCustomMiddleware();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
