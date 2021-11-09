using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using BAL.Helpers;
using CommonDomain.DataModels;
using Domains.Models;
using DTOs.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Web;
using Syncfusion.Blazor;
using System.Globalization;
using System.Text;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("init main");
    #region .NET 6 程式進入點程式碼
    var builder = WebApplication.CreateBuilder(args);

    #region .NET 5 專案內的 CreateHostBuilder
    IHostBuilder hostBuilder=builder.Host;
    hostBuilder.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    }).UseNLog();
    #endregion

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddSingleton<WeatherForecastService>();

    #region .NET 5 內的 ConfigureServices
    #region 顯示所讀取的 Configuration 內容值
    bool showImportanceConfigurationInforation = Convert
        .ToBoolean(builder.Configuration[AppSettingHelper.ShowImportanceConfigurationInforation]);
    if (showImportanceConfigurationInforation)
    {
        AllConfigurationHelper.Show(builder.Configuration);
    }
    #endregion

    #region Blazor & Razor Page 用到服務
    builder.Services.AddRazorPages();
    #region Blazor Server 註冊服務，正式部署下，是否要顯示明確的例外異常資訊
    bool emergenceDebugStatus = Convert.ToBoolean(builder.Configuration[AppSettingHelper.EmergenceDebug]);
    if (emergenceDebugStatus == true)
    {
        Console.WriteLine($"啟用正式部署可以顯示錯誤詳細資訊");
        builder.Services.AddServerSideBlazor()
            .AddCircuitOptions(e =>
            {
                e.DetailedErrors = true;
            });
    }
    else
    {
        builder.Services.AddServerSideBlazor();
    }
    #endregion
    #endregion

    #region Syncfusion 元件與多國語言服務
    // Localization https://github.com/syncfusion/blazor-locale
    // 各國語言代碼 https://en.wikipedia.org/wiki/Language_localisation
    // Set the resx file folder path to access
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddSyncfusionBlazor();
    // Register the Syncfusion locale service to customize the  SyncfusionBlazor component locale culture
    builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
    builder.Services.Configure<RequestLocalizationOptions>(options =>
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
    string connectionString = builder.Configuration.GetConnectionString(MagicHelper.DefaultConnectionString);
    builder.Services.AddDbContext<BackendDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(
        MagicHelper.DefaultConnectionString)), ServiceLifetime.Transient);
    builder.Services.AddCustomServices();
    //builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
    #endregion

    #region 加入設定強型別注入宣告
    builder.Services.Configure<BackendTokenConfiguration>(builder.Configuration
        .GetSection(AppSettingHelper.Tokens));
    builder.Services.Configure<BackendSmtpClientInformation>(builder.Configuration
        .GetSection(AppSettingHelper.BackendSmtpClientInformation));
    #endregion

    #region 加入使用 Cookie & JWT 認證需要的宣告
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
    });
    builder.Services.AddAuthentication(
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
                ValidIssuer = builder.Configuration[AppSettingHelper.ValidIssuer],
                ValidAudience = builder.Configuration[AppSettingHelper.ValidAudience],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration[AppSettingHelper.ValidAudience])),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
            };
            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = context.Exception.Message;
                    APIResult apiResult = JWTTokenFailHelper.GetFailResult(context.Exception);

                    context.Response.ContentType = "application/json";
                    context.Response.WriteAsync(JsonConvert.SerializeObject(apiResult)).Wait();
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
    builder.Services.AddControllers();
    #endregion

    #region 修正 Web API 的 JSON 處理
    builder.Services.AddMvc()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new DefaultContractResolver());

    builder.Services.AddControllers()
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
    builder.Services.AddSwaggerGen();
    #endregion

    #region 加入背景服務
    var EnableKeepAliveEndpoint = Convert.ToBoolean(builder.Configuration[AppSettingHelper.EnableKeepAliveEndpoint]);
    if (EnableKeepAliveEndpoint == true)
    {
        builder.Services.AddHostedService<KeepAliveHostedService>();
    }
    builder.Services.AddHostedService<PasswordPolicyHostedService>();
    builder.Services.AddHostedService<SendingMailHostedService>();
    #endregion

    #region 使用 HttpContext
    builder.Services.AddHttpContextAccessor();
    #endregion

    #region 相關選項模式
    builder.Services.Configure<BackendCustomNLog>(builder.Configuration
        .GetSection(nameof(BackendCustomNLog)));
    #endregion
    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
    #endregion
}
catch (Exception exception)
{
    //NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}






