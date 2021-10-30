using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AC08
{
    public class BackgroundHostedService : IHostedService
    {
        private readonly ILogger<BackgroundHostedService> logger;
        DateTime StartupTime = DateTime.Now;
        Task BackgroundHostedServiceTask;
        CancellationTokenSource cancellationTokenSource = new();

        public BackgroundHostedService(ILogger<BackgroundHostedService> logger)
        {
            this.logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = new CancellationTokenSource();
            logger.LogInformation($"託管服務的背景工作 開始啟動");

            var backgroundService = Task.Run(async () =>
            {
                try
                {
                    StartupTime = DateTime.Now;
                    await Task.Delay(5000);

                    while (cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        logger.LogInformation($"託管服務的背景工作 正在執行中");
                        logger.LogInformation($"託管服務的背景工作 等候下一次的執行");
                        await Task.Delay(60*1000, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation($"託管服務的背景工作 準備正常離開中");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, $"託管服務的背景工作 產生例外異常");
                }
            });
            BackgroundHostedServiceTask = backgroundService;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            for (int i = 0; i < 10; i++)
            {
                if (BackgroundHostedServiceTask.IsCompleted == true)
                    break;
#pragma warning disable CA2016 // 將 'CancellationToken' 參數傳遞給使用該參數的方法
                await Task.Delay(500);
#pragma warning restore CA2016 // 將 'CancellationToken' 參數傳遞給使用該參數的方法
            }
            TimeSpan timeSpan = DateTime.Now - StartupTime;
            logger.LogInformation($"託管服務的背景工作 即將停止，共花費 {timeSpan}");

            return;
        }
    }
}
