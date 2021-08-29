using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Services
{
    public class PasswordPolicyHostedService : IHostedService
    {
        //public PasswordPolicyHostedService(ILogger<PasswordPolicyHostedService> logger,
        //    IServer server, IConfiguration configuration, PasswordPolicyService passwordPolicyService)
        public PasswordPolicyHostedService(ILogger<PasswordPolicyHostedService> logger,
            IServer server, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public ILogger<PasswordPolicyHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }
        public IServiceScopeFactory ServiceScopeFactory { get; }

        int checkCycle = 60 * 60;
        DateTime StartupTime = DateTime.Now;
        Task PasswordPolicyTask;
        CancellationTokenSource cancellationTokenSource = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Logger.LogInformation($"Password Policy Check 服務開始啟動");

            var backgroundService = Task.Run(async () =>
            {
                #region 檢查使用者是否要強制變更密碼
                try
                {
                    StartupTime = DateTime.Now;

                    await Task.Delay(15000);

                    var scope = ServiceScopeFactory.CreateScope();
                    IPasswordPolicyService passwordPolicyService = scope.ServiceProvider.GetRequiredService<IPasswordPolicyService>();

                    HttpClient client = new();
                    while (cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        var dateOffset = DateTime.UtcNow.AddHours(8);
                        TimeSpan timeSpan = DateTime.Now - StartupTime;
                        // Todo : 這樣的用法要學起來
                        Logger.LogInformation($@"Password Policy Check 檢查密碼是否要定期更新 ({timeSpan}) {dateOffset:yyyy-MM-dd HH:mm:ss}");

                        try
                        {
                            await passwordPolicyService.CheckPasswordAge();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, $"Password Policy Check 檢查密碼是否要定期更新 發生例外異常");
                        }
                        await Task.Delay(checkCycle * 1000, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger.LogInformation($"Keep alive 服務準備正常離開中");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, $"Keep alive 服務產生例外異常");
                }
                #endregion
            });
            PasswordPolicyTask = backgroundService;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            for (int i = 0; i < 10; i++)
            {
                if (PasswordPolicyTask.IsCompleted == true)
                    break;
#pragma warning disable CA2016 // 將 'CancellationToken' 參數傳遞給使用該參數的方法
                await Task.Delay(500);
#pragma warning restore CA2016 // 將 'CancellationToken' 參數傳遞給使用該參數的方法
            }
            TimeSpan timeSpan = DateTime.Now - StartupTime;
            Logger.LogInformation($"Keep alive 服務即將停止，共花費 {timeSpan}");

            return;
        }
    }
}
