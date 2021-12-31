using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BAL.Helpers;
using Backend.Models;

namespace Backend.Services
{
    public class KeepAliveHostedService : IHostedService
    {
        public KeepAliveHostedService(ILogger<KeepAliveHostedService> logger,
            IServer server, IConfiguration configuration, BackgroundExecuteMode backgroundExecuteMode)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
            BackgroundExecuteMode = backgroundExecuteMode;
        }

        public ILogger<KeepAliveHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }
        public BackgroundExecuteMode BackgroundExecuteMode { get; }

        DateTime lastLogTime;
        readonly int keepAliveCycle = 360; // 約六分鐘
        readonly int checkCycle = 60;
        DateTime StartupTime = DateTime.Now;
        Task keepAliveTask;
        CancellationTokenSource cancellationTokenSource = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Logger.LogInformation($"Keep alive 服務開始啟動");
            var backgroundService = Task.Factory.StartNew(async () =>
            {
                #region 確保 IIS 不會自動停止的背景服務
                try
                {
                    await Task.Delay(75000, cancellationTokenSource.Token);

                    StartupTime = DateTime.Now;
                    lastLogTime = DateTime.Now;
                    HttpClient client = new();
                    while (cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        #region 若在進行資料庫重建與初始化的時候，需要暫緩執行背景工作
                        while (BackgroundExecuteMode.IsInitialization == true)
                        {
                            await Task.Delay(60000, cancellationTokenSource.Token);
                        }
                        #endregion

                        var nextTime = lastLogTime.AddSeconds(keepAliveCycle);
                        if (DateTime.Now > nextTime)
                        {
                            var address = Configuration[AppSettingHelper.KeepAliveEndpoint];
                            var dateOffset = DateTime.UtcNow.AddHours(8);
                            TimeSpan timeSpan = DateTime.Now - StartupTime;
                            // Todo : 這樣的用法要學起來
                            Logger.LogInformation($@"Keep alive 確保IIS服務不會自動關閉 ({timeSpan}) {dateOffset:yyyy-MM-dd HH:mm:ss}");

                            try
                            {
                                await client.GetStringAsync($"{address}/Login");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogWarning(ex, $"Keep alive 連線到 {address}/Login 發生例外異常");
                            }
                            lastLogTime = DateTime.Now;
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
