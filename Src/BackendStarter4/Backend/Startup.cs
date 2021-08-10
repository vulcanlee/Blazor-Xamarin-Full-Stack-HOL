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
            #region 顯示所讀取的 Configuration 內容值
            bool showImportanceConfigurationInforation = Convert.ToBoolean(Configuration["ShowImportanceConfigurationInforation"]);
            if (showImportanceConfigurationInforation)
            {
                AllConfigurationHelper.Show(Configuration);
            }
            #endregion

            #region Blazor & Razor Page 用到服務
            services.AddRazorPages();
            #region Blazor Server 註冊服務，正式部署下，是否要顯示明確的例外異常資訊
            bool emergenceDebugStatus = Convert.ToBoolean(Configuration["EmergenceDebug"]);
            if (emergenceDebugStatus == true)
            {
                Console.WriteLine($"啟用正式部署可以顯示錯誤詳細資訊");
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

            #region Syncfusion 元件與多國語言服務
            // Localization https://github.com/syncfusion/blazor-locale
            // 各國語言代碼 https://en.wikipedia.org/wiki/Language_localisation
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

            #region EF Core & AutoMapper 使用的宣告
            string connectionString = Configuration.GetConnectionString(MagicHelper.DefaultConnectionString);
            services.AddDbContext<BackendDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(
                MagicHelper.DefaultConnectionString)));
            services.AddCustomServices();
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
            #endregion

            #region 加入設定強型別注入宣告
            services.Configure<TokenConfiguration>(Configuration.GetSection("Tokens"));
            #endregion

            #region 加入使用 Cookie & JWT 認證需要的宣告
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

            #region 新增控制器和 API 相關功能的支援，但不會加入 views 或 pages
            services.AddControllers();
            #endregion

            #region 修正 Web API 的 JSON 處理
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

            #region 設定 Swagger 中介軟體
            services.AddSwaggerGen();
            #endregion

            #region 加入背景服務
            var EnableKeepAliveEndpoint = Convert.ToBoolean(Configuration["EnableKeepAliveEndpoint"]);
            if (EnableKeepAliveEndpoint == true)
            {
                services.AddHostedService<KeepAliveHostedService>();
            }
            #endregion

            #region 使用 HttpContext
            services.AddHttpContextAccessor();
            #endregion

            #region 相關選項模式
            services.Configure<CustomNLog>(Configuration
                .GetSection(nameof(CustomNLog)));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger, IOptions<CustomNLog> optionsCustomNLog)
        {
            #region 當呼叫 API ( /api/someController ) 且該服務端點不存在的時候，將會替換網頁為 404 的 APIResult 訊息
            app.UseApiNotFoundPageToAPIResult();
            #endregion

            #region 宣告 NLog 要使用到的變數內容
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

            #region 開發模式的設定
            bool emergenceDebugStatus = Convert.ToBoolean(Configuration["EmergenceDebug"]);
            if (emergenceDebugStatus == true) logger.LogInformation("緊急除錯模式 : 啟用");
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

            #region 啟用 Swagger 中介軟體
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

            #region 指定要使用 Cookie & 使用者認證的中介軟體
            app.UseCookiePolicy();
            app.UseAuthentication();
            #endregion

            #region 指定使用授權檢查的中介軟體
            app.UseAuthorization();
            #endregion

            #region 取得來源 IP
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
