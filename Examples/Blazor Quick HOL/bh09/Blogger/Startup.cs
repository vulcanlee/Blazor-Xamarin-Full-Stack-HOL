using Blogger.Data;
using Blogger.Helpers;
using Blogger.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            #region �ŧi BlogDbContext �|�Ψ쪺�A��
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDbContext>(
                options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            #endregion

            #region �[�J�Ȼs�ƪA�Ȫ`�J�ŧi
            services.AddCustomDependency();
            #endregion

            #region �[�J�ϥ� Cookie & JWT �{�һݭn���ŧi
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddAuthentication(
                MagicHelper.CookieAuthenticationScheme)
                .AddCookie(MagicHelper.CookieAuthenticationScheme);
            #endregion

            #region �ϥ� HttpContext
            services.AddHttpContextAccessor();
            #endregion

            #region �s�W����M API �����\�઺�䴩�A�����|�[�J views �� pages
            services.AddControllers();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            #region ���w�n�ϥΨϥΪ̻{�Ҫ������n��
            app.UseAuthentication();
            #endregion

            #region ���w�ϥα��v�ˬd�������n��
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                #region Adds endpoints for controller actions to the IEndpointRouteBuilder without specifying any routes.
                endpoints.MapControllers();
                #endregion

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
