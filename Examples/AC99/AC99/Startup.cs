using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AC99
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 這裡加入 Blazor 需要用到的相依性注入宣告
            services.AddRazorPages();
            services.AddServerSideBlazor();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 要加入這個敘述，以便可以執行 SignalR 的 JavaScript 程式
            app.UseStaticFiles();
            #endregion

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                #region 啟動 SignalR 服務與註冊最終路由端點
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                #endregion
            });
        }
    }
}
