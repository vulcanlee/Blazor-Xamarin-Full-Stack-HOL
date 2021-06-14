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
            #region Case 1 �ۦ�Ȼs�����n�� 1
            app.Use(async (context, next) =>
            {
                // �I�s�U�@�Ӥ����n��n���檺���e�A�b�o�̤��n�g�J���󤺮e�� HTTP Responet �^����
                Console.WriteLine($"�I�s�� 1 �ӫȻs�Ƥ����n��");
                Console.WriteLine($"���Ѹ��| {context.Request.Path}");

                await next.Invoke();

                // �۳o�̶i��g�J��x�Ψ�L�u�@�A�b�o�̤��n�g�J���󤺮e�� HTTP Responet �^����
                Console.WriteLine($"��������� 1 �ӫȻs�Ƥ����n��");
                Console.WriteLine($"�^�����A�X {context.Response.StatusCode}");
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

            #region Case 2 �[�J�B�~�������R�A�ɮת��ﶵ StaticFileOptions 
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "MyImages")),
                RequestPath = "/Images"
            });
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            #region Case 3 �ۦ�Ȼs�����n�� 2
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"�I�s�� 2 �ӫȻs�Ƥ����n��");
                Console.WriteLine($"���Ѹ��| {context.Request.Path}");

                // ����o�� ���I �ȯ���ݨ� "Hello World!" ��r
                if (context.Request.Path == "/Vulcan")
                {
                    await context.Response.WriteAsync("Hello World!");
                    return;
                }
                
                await next.Invoke();

                // �۳o�̶i��g�J��x�Ψ�L�u�@�A�b�o�̤��n�g�J���󤺮e�� HTTP Responet �^����
                Console.WriteLine($"��������� 2 �ӫȻs�Ƥ����n��");
            });
            #endregion

            #region Case 4 �ۦ�Ȼs�����n�� 3
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
