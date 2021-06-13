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
            #region �o�̥[�J Blazor �ݭn�Ψ쪺�̩ۨʪ`�J�ŧi
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

            #region �n�[�J�o�ӱԭz�A�H�K�i�H���� SignalR �� JavaScript �{��
            app.UseStaticFiles();
            #endregion

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                #region �Ұ� SignalR �A�ȻP���U�̲׸��Ѻ��I
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                #endregion
            });
        }
    }
}
