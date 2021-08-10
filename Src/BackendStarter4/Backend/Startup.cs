using Backend.Helpers;
using Backend.Middlewares;
using Backend.Models;
using Backend.Services;
using DTOs.DataModels;
using Domains.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using BAL.Helpers;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;

namespace Backend
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
            #region ��ܩ�Ū���� Configuration ���e��
            bool showImportanceConfigurationInforation = Convert.ToBoolean(Configuration["ShowImportanceConfigurationInforation"]);
            if (showImportanceConfigurationInforation)
            {
                AllConfigurationHelper.Show(Configuration);
            }
            #endregion

            #region Blazor & Razor Page �Ψ�A��
            services.AddRazorPages();
            #region Blazor Server ���U�A�ȡA�������p�U�A�O�_�n��ܩ��T���ҥ~���`��T
            bool emergenceDebugStatus = Convert.ToBoolean(Configuration["EmergenceDebug"]);
            if (emergenceDebugStatus == true)
            {
                Console.WriteLine($"�ҥΥ������p�i�H��ܿ��~�ԲӸ�T");
                services.AddServerSideBlazor()
                    .AddCircuitOptions(e =>
                    {
                        e.DetailedErrors = true;
                    });
            }
            else
            {
                services.AddServerSideBlazor();
            }
            #endregion
            #endregion

            #region Syncfusion ����P�h��y���A��
            // Localization https://github.com/syncfusion/blazor-locale
            // �U��y���N�X https://en.wikipedia.org/wiki/Language_localisation
            // Set the resx file folder path to access
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSyncfusionBlazor();
            // Register the Syncfusion locale service to customize the  SyncfusionBlazor component locale culture
            services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // Define the list of cultures your app will support
                var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-TW"),
                };
                // Set the default culture
                options.DefaultRequestCulture = new RequestCulture("zh-TW");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            #endregion

            #region EF Core & AutoMapper �ϥΪ��ŧi
            string connectionString = Configuration.GetConnectionString(MagicHelper.DefaultConnectionString);
            services.AddDbContext<BackendDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(
                MagicHelper.DefaultConnectionString)));
            services.AddCustomServices();
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
            #endregion

            #region �[�J�]�w�j���O�`�J�ŧi
            services.Configure<TokenConfiguration>(Configuration.GetSection("Tokens"));
            #endregion

            #region �[�J�ϥ� Cookie & JWT �{�һݭn���ŧi
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddAuthentication(
                MagicHelper.CookieAuthenticationScheme)
                .AddCookie(MagicHelper.CookieAuthenticationScheme)
                .AddJwtBearer(MagicHelper.JwtBearerAuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Tokens:ValidIssuer"],
                        ValidAudience = Configuration["Tokens:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                       {
                           context.Response.OnStarting(async () =>
                           {
                               context.Response.StatusCode = 401;
                               context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = context.Exception.Message;
                               APIResult apiResult = JWTTokenFailHelper.GetFailResult(context.Exception);

                               context.Response.ContentType = "application/json";
                               await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
                           });
                           return Task.CompletedTask;
                       },
                        OnChallenge = context =>
                        {
                            ////context.HandleResponse();
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            //Console.WriteLine("OnTokenValidated: " +
                            //    context.SecurityToken);
                            return Task.CompletedTask;
                        }

                    };
                });
            #endregion

            #region �s�W����M API �����\�઺�䴩�A�����|�[�J views �� pages
            services.AddControllers();
            #endregion

            #region �ץ� Web API �� JSON �B�z
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            #endregion

            #region �]�w Swagger �����n��
            services.AddSwaggerGen();
            #endregion

            #region �[�J�I���A��
            var EnableKeepAliveEndpoint = Convert.ToBoolean(Configuration["EnableKeepAliveEndpoint"]);
            if (EnableKeepAliveEndpoint == true)
            {
                services.AddHostedService<KeepAliveHostedService>();
            }
            #endregion

            #region �ϥ� HttpContext
            services.AddHttpContextAccessor();
            #endregion

            #region �����ﶵ�Ҧ�
            services.Configure<CustomNLog>(Configuration
                .GetSection(nameof(CustomNLog)));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger, IOptions<CustomNLog> optionsCustomNLog)
        {
            #region ��I�s API ( /api/someController ) �B�ӪA�Ⱥ��I���s�b���ɭԡA�N�|���������� 404 �� APIResult �T��
            app.UseApiNotFoundPageToAPIResult();
            #endregion

            #region �ŧi NLog �n�ϥΨ쪺�ܼƤ��e
            LogManager.Configuration.Variables["LogRootPath"] =
                optionsCustomNLog.Value.LogRootPath;
            LogManager.Configuration.Variables["AllLogMessagesFilename"] =
                optionsCustomNLog.Value.AllLogMessagesFilename;
            LogManager.Configuration.Variables["AllWebDetailsLogMessagesFilename"] =
                optionsCustomNLog.Value.AllWebDetailsLogMessagesFilename;
            #endregion

            #region Syncfusion License Registration
            string key = Configuration["Syncfusion:License"];
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
            #endregion

            #region Localization
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
            #endregion

            #region Set X-FRAME-OPTIONS in ASP.NET Core
            // https://blog.johnwu.cc/article/asp-net-core-response-header.html
            // https://dotnetcoretutorials.com/2017/01/08/set-x-frame-options-asp-net-core/
            // https://developer.mozilla.org/zh-TW/docs/Web/HTTP/Headers/X-Frame-Options
            // https://blog.darkthread.net/blog/remove-iis-response-server-header/
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("X-Frame-Options");
                context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
                await next();
            });
            #endregion

            #region �}�o�Ҧ����]�w
            bool emergenceDebugStatus = Convert.ToBoolean(Configuration["EmergenceDebug"]);
            if (emergenceDebugStatus == true) logger.LogInformation("��氣���Ҧ� : �ҥ�");
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

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            #endregion

            app.UseStaticFiles();

            #region �ҥ� Swagger �����n��
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API V1");
            });
            #endregion

            app.UseRouting();

            #region ���w�n�ϥ� Cookie & �ϥΪ̻{�Ҫ������n��
            app.UseCookiePolicy();
            app.UseAuthentication();
            #endregion

            #region ���w�ϥα��v�ˬd�������n��
            app.UseAuthorization();
            #endregion

            #region ���o�ӷ� IP
            app.UseMyMiddleware();
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
