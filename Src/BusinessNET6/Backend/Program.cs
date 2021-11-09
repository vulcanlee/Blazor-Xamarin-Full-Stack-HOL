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
    #region .NET 6 �{���i�J�I�{���X
    var builder = WebApplication.CreateBuilder(args);

    #region .NET 5 �M�פ��� CreateHostBuilder
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

    #region .NET 5 ���� ConfigureServices
    #region ��ܩ�Ū���� Configuration ���e��
    bool showImportanceConfigurationInforation = Convert
        .ToBoolean(builder.Configuration[AppSettingHelper.ShowImportanceConfigurationInforation]);
    if (showImportanceConfigurationInforation)
    {
        AllConfigurationHelper.Show(builder.Configuration);
    }
    #endregion

    #region Blazor & Razor Page �Ψ�A��
    builder.Services.AddRazorPages();
    #region Blazor Server ���U�A�ȡA�������p�U�A�O�_�n��ܩ��T���ҥ~���`��T
    bool emergenceDebugStatus = Convert.ToBoolean(builder.Configuration[AppSettingHelper.EmergenceDebug]);
    if (emergenceDebugStatus == true)
    {
        Console.WriteLine($"�ҥΥ������p�i�H��ܿ��~�ԲӸ�T");
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

    #region Syncfusion ����P�h��y���A��
    // Localization https://github.com/syncfusion/blazor-locale
    // �U��y���N�X https://en.wikipedia.org/wiki/Language_localisation
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

    #region EF Core & AutoMapper �ϥΪ��ŧi
    string connectionString = builder.Configuration.GetConnectionString(MagicHelper.DefaultConnectionString);
    builder.Services.AddDbContext<BackendDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(
        MagicHelper.DefaultConnectionString)), ServiceLifetime.Transient);
    builder.Services.AddCustomServices();
    //builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
    #endregion

    #region �[�J�]�w�j���O�`�J�ŧi
    builder.Services.Configure<BackendTokenConfiguration>(builder.Configuration
        .GetSection(AppSettingHelper.Tokens));
    builder.Services.Configure<BackendSmtpClientInformation>(builder.Configuration
        .GetSection(AppSettingHelper.BackendSmtpClientInformation));
    #endregion

    #region �[�J�ϥ� Cookie & JWT �{�һݭn���ŧi
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

    #region �s�W����M API �����\�઺�䴩�A�����|�[�J views �� pages
    builder.Services.AddControllers();
    #endregion

    #region �ץ� Web API �� JSON �B�z
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

    #region �]�w Swagger �����n��
    builder.Services.AddSwaggerGen();
    #endregion

    #region �[�J�I���A��
    var EnableKeepAliveEndpoint = Convert.ToBoolean(builder.Configuration[AppSettingHelper.EnableKeepAliveEndpoint]);
    if (EnableKeepAliveEndpoint == true)
    {
        builder.Services.AddHostedService<KeepAliveHostedService>();
    }
    builder.Services.AddHostedService<PasswordPolicyHostedService>();
    builder.Services.AddHostedService<SendingMailHostedService>();
    #endregion

    #region �ϥ� HttpContext
    builder.Services.AddHttpContextAccessor();
    #endregion

    #region �����ﶵ�Ҧ�
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






