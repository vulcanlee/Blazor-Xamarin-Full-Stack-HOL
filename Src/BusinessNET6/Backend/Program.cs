using Backend.Data;
using Backend.Helpers;
using Backend.Middlewares;
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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using Syncfusion.Blazor;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.HttpOverrides;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("init main");
    #region .NET 6 �{���i�J�I�{���X
    var builder = WebApplication.CreateBuilder(args);

    #region .NET 5 �M�פ��� CreateHostBuilder
    IHostBuilder hostBuilder = builder.Host;

    hostBuilder.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
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
    string connectionStringSQLite = builder.Configuration.GetConnectionString(MagicHelper.DefaultSQLiteConnectionString);
    bool UseSQLite = Convert.ToBoolean(builder.Configuration["BackendSystemAssistant:UseSQLite"]);
    if (UseSQLite == false)
    {
        builder.Services.AddDbContext<BackendDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString(
            MagicHelper.DefaultConnectionString)), ServiceLifetime.Transient);
    }
    else
    {
        builder.Services.AddDbContext<BackendDBContext>(options =>
        options.UseSqlite(connectionStringSQLite), ServiceLifetime.Transient);
    }
    builder.Services.AddCustomServices();
    builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapping>());
    #endregion

    #region �[�J�]�w�j���O�`�J�ŧi
    builder.Services.Configure<BackendTokenConfiguration>(builder.Configuration
        .GetSection(AppSettingHelper.Tokens));
    builder.Services.Configure<BackendSmtpClientInformation>(builder.Configuration
        .GetSection(AppSettingHelper.BackendSmtpClientInformation));
    builder.Services.Configure<BackendInitializer>(builder.Configuration
        .GetSection(AppSettingHelper.BackendInitializer));
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

    // �I���A�ȬO�_�ݭn�Ȱ�����
    builder.Services.AddSingleton<BackgroundExecuteMode>();
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

    #region .NET 5 �� Configure
    #region ��I�s API ( /api/someController ) �B�ӪA�Ⱥ��I���s�b���ɭԡA�N�|���������� 404 �� APIResult �T��
    app.UseApiNotFoundPageToAPIResult();
    #endregion
    IOptions<BackendCustomNLog> optionsCustomNLog = app.Services.GetRequiredService<IOptions<BackendCustomNLog>>();
    #region �ŧi NLog �n�ϥΨ쪺�ܼƤ��e
    LogManager.Configuration.Variables["LogRootPath"] =
        optionsCustomNLog.Value.LogRootPath;
    LogManager.Configuration.Variables["AllLogMessagesFilename"] =
        optionsCustomNLog.Value.AllLogMessagesFilename;
    LogManager.Configuration.Variables["AllWebDetailsLogMessagesFilename"] =
        optionsCustomNLog.Value.AllWebDetailsLogMessagesFilename;
    #endregion

    #region Syncfusion License Registration
    string key = builder.Configuration[AppSettingHelper.SyncfusionLicense];
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
    #endregion

    #region Localization
    app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
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

    #region �Y�b ��氣�� �Ҧ��U�A�C�X Configuration ���o����ڭ�
    emergenceDebugStatus = Convert.ToBoolean(builder.Configuration[AppSettingHelper.EmergenceDebug]);
    ILogger<Program> loggerEmergence = app.Services.GetRequiredService<ILogger<Program>>();
    if (emergenceDebugStatus == true)
    {
        loggerEmergence.LogInformation("��氣���Ҧ� : �ҥ�");
        var allConfiguration = JsonConvert
        .SerializeObject(IConfigrationToJsonHelp.Serialize(builder.Configuration),
        Formatting.Indented);
        loggerEmergence.LogInformation(allConfiguration);
    }
    #endregion

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }
    #endregion

    #region �w��ثe���n�D���|�ҥ��R�A�ɮתA��
    app.UseStaticFiles();

    #region �Ϥ��R�A�ɮ�
    // https://stackoverflow.com/questions/1029740/get-mime-type-from-filename-extension
    UploadImageHelper.PrepareUploadImageFolder(builder.Configuration).Wait();
    var providerImage = new FileExtensionContentTypeProvider();
    // Add new mappings
    providerImage.Mappings[".jpg"] = "image/jpeg";
    providerImage.Mappings[".jpeg"] = "image/jpeg";
    providerImage.Mappings[".mp4"] = "video/mp4";
    string keyUploadImagePath = builder.Configuration[AppSettingHelper.UploadImagePath];
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(
         Path.Combine(keyUploadImagePath, "Images")),
        RequestPath = "/Images",
        ContentTypeProvider = providerImage
    });
    #endregion
    #endregion

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

    #region �N���ѹ����s�W�ܤ����n��޽u
    app.UseRouting();
    #endregion

    #region ���w�n�ϥΨϥΪ̻{�Ҫ������n��
    app.UseCookiePolicy();
    app.UseAuthentication();
    #endregion

    #region ���w�ϥα��v�ˬd�������n��
    app.UseAuthorization();
    #endregion

    #region ���o�ӷ� IP
    // https://www.coderperfect.com/how-do-i-get-client-ip-address-in-asp-net-core/
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseMyMiddleware();

    #endregion

    #region �ۭq�@��Middleware
    app.Use(async (context, next) =>
    {
        //Doworkthatdoesn'twritetotheResponse.
        var url = $"[{context.Request.Method}] {context.Request.Scheme}://" +
        $"{context.Request.Host.Value}{context.Request.Path.Value}";
        Console.WriteLine(url);
        await next.Invoke();
        //Dologgingorotherworkthatdoesn'twritetotheResponse.
    });
    #endregion

    #region �N���I����s�W�ܤ����n��޽u
    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
    #endregion

    #endregion


    //// Configure the HTTP request pipeline.
    //if (!app.Environment.IsDevelopment())
    //{
    //    app.UseExceptionHandler("/Error");
    //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //    app.UseHsts();
    //}

    //app.UseHttpsRedirection();

    //app.UseStaticFiles();

    //app.UseRouting();

    //app.MapBlazorHub();
    //app.MapFallbackToPage("/_Host");

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






