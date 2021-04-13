using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Backend.Services
{
    public class KeepAliveHostedService : IHostedService
    {
        public KeepAliveHostedService(ILogger<KeepAliveHostedService> logger,
            IServer server, IConfiguration configuration)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
        }

        public ILogger<KeepAliveHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }

        DateTime lastLogTime;
        int keepAliveCycle = 360; // 約六分鐘
        int checkCycle = 60;
        DateTime StartupTime = DateTime.Now;
        Task keepAliveTask;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Logger.LogInformation($"Keep alive 服務開始啟動");
            var backgroundService = Task.Factory.StartNew(async () =>
            {
                #region 確保 IIS 不會自動停止的背景服務
                try
                {
                    StartupTime = DateTime.Now;
                    lastLogTime = DateTime.Now;
                    HttpClient client = new HttpClient();
                    while (cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        var nextTime = lastLogTime.AddSeconds(keepAliveCycle);
                        if (DateTime.Now > nextTime)
                        {
                            var address = Configuration["KeepAliveEndpoint"];
                            var dateOffset = DateTime.UtcNow.AddHours(8);
                            TimeSpan timeSpan = DateTime.Now - StartupTime;
                            Logger.LogInformation($@"Keep alive 確保IIS服務不會自動關閉 ({timeSpan}) {dateOffset.ToString("yyyy-MM-dd HH:mm:ss")}");

                            try
                            {
                                await client.GetStringAsync($"{address}/Login");
                            }
                            catch(Exception ex)
                            {
                                Logger.LogWarning(ex, $"Keep alive 連線到 {address}/Login 發生例外異常");
                            }
                            lastLogTime = DateTime.Now;
                        }
                        await Task.Delay(checkCycle * 1000, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Logger.LogInformation($"Keep alive 服務準備正常離開中");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, $"Keep alive 服務產生例外異常");
                }
                #endregion
            }, TaskCreationOptions.LongRunning);
            keepAliveTask = backgroundService.Result;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            for (int i = 0; i < 10; i++)
            {
                if (keepAliveTask.IsCompleted == true)
                    break;
                await Task.Delay(500);
            }
            TimeSpan timeSpan = DateTime.Now - StartupTime;
            Logger.LogInformation($"Keep alive 服務即將停止，共花費 {timeSpan}");

            return;
        }
    }
}
