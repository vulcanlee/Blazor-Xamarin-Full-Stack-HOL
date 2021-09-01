using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BAL.Helpers;
using Microsoft.Extensions.Options;
using Backend.Models;
using CommonDomain.DataModels;
using Backend.Events;

namespace Backend.Services
{
    public class SendingMailHostedService : IHostedService
    {
        public SendingMailHostedService(ILogger<SendingMailHostedService> logger,
            IServer server, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory,
            IOptions<SmtpClientInformation> smtpClientInformation, SystemBroadcast systemBroadcast)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
            ServiceScopeFactory = serviceScopeFactory;
            SystemBroadcast = systemBroadcast;
            SmtpClientInformation = smtpClientInformation.Value;
        }

        public ILogger<SendingMailHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }
        public IServiceScopeFactory ServiceScopeFactory { get; }
        public SystemBroadcast SystemBroadcast { get; }
        public SmtpClientInformation SmtpClientInformation { get; }

        int checkCycle = 60 * 60;
        DateTime StartupTime = DateTime.Now;
        Task PasswordPolicyTask;
        CancellationTokenSource cancellationTokenSource = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Logger.LogInformation($"寄送郵件 服務開始啟動");
            int SendingMailInterval = Convert.ToInt32(Configuration["SendingMailInterval"]);

            var backgroundService = Task.Run(async () =>
            {
                #region 檢查使用者是否要發送電子郵件
                try
                {
                    StartupTime = DateTime.Now;
                    Random random = new Random();
                    var firstDelay = random.Next(60 * 1000, 3 * 60 * 1000);

#if DEBUG
                    await Task.Delay(5000);
#else
                    await Task.Delay(firstDelay);
#endif

                    SmtpHelper.Initialization(SmtpClientInformation);
                    while (cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        var scope = ServiceScopeFactory.CreateScope();
                        IMailQueueService mailQueueService = scope.ServiceProvider.GetRequiredService<IMailQueueService>();

                        var dateOffset = DateTime.UtcNow.AddHours(8);
                        TimeSpan timeSpan = DateTime.Now - StartupTime;

                        try
                        {
                            var waitSendingMails = await mailQueueService.GetNotSentAsync();
                            cancellationTokenSource.Token.ThrowIfCancellationRequested();

#region 寄送郵件
                            var allEmail = await mailQueueService.GetNotSentAsync();
                            SendEmailModel sendEmailModel = new SendEmailModel();
                            foreach (var email in allEmail)
                            {
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                                sendEmailModel.From = SmtpClientInformation.UserName;
                                sendEmailModel.To = email.To;
                                sendEmailModel.Subject = email.Subject;
                                sendEmailModel.Body = email.Body;
                                var successful = await SmtpHelper.SendSMTP(sendEmailModel, cancellationTokenSource.Token);
                                email.SendTimes++;
                                email.SendedAt = DateTime.Now;
                                if (successful == false && email.SendTimes > MagicHelper.MaxEmailResend)
                                {
                                    email.Status = MagicHelper.MailStatus失敗;
                                }
                                else
                                {
                                    email.Status = MagicHelper.MailStatus成功;
                                }
                                await mailQueueService.UpdateAsync(email);
                            }
#endregion
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, $"寄送郵件 發生例外異常");
                        }

                        scope.Dispose();
                        await Task.Delay(SendingMailInterval, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger.LogInformation($"寄送郵件 服務準備正常離開中");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, $"寄送郵件 服務產生例外異常");
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
