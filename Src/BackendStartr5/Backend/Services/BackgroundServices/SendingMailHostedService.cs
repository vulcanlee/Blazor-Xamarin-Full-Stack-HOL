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
using Backend.Helpers;
using Backend.AdapterModels;

namespace Backend.Services
{
    public class SendingMailHostedService : IHostedService
    {
        public SendingMailHostedService(ILogger<SendingMailHostedService> logger,
            IServer server, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory,
            IOptions<BackendSmtpClientInformation> smtpClientInformation, SystemBroadcast systemBroadcast,
            BackgroundExecuteMode backgroundExecuteMode)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
            ServiceScopeFactory = serviceScopeFactory;
            SystemBroadcast = systemBroadcast;
            BackgroundExecuteMode = backgroundExecuteMode;
            SmtpClientInformation = smtpClientInformation.Value;
        }

        public ILogger<SendingMailHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }
        public IServiceScopeFactory ServiceScopeFactory { get; }
        public SystemBroadcast SystemBroadcast { get; }
        public BackgroundExecuteMode BackgroundExecuteMode { get; }
        public BackendSmtpClientInformation SmtpClientInformation { get; }

        DateTime StartupTime = DateTime.Now;
        Task PasswordPolicyTask;
        CancellationTokenSource cancellationTokenSource = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            int smtpExceptionTimes = 0;
            cancellationTokenSource = new CancellationTokenSource();
            Logger.LogInformation($"寄送郵件 服務開始啟動");
            int SendingMailInterval = Convert.ToInt32(Configuration[AppSettingHelper.SendingMailInterval]);

            var backgroundService = Task.Run(async () =>
            {
                #region 檢查使用者是否要發送電子郵件
                try
                {
                    await Task.Delay(120000, cancellationTokenSource.Token);

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
                        #region 若在進行資料庫重建與初始化的時候，需要暫緩執行背景工作
                        while (BackgroundExecuteMode.IsInitialization == true)
                        {
                            await Task.Delay(60000, cancellationTokenSource.Token);
                        }
                        #endregion

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
                                smtpExceptionTimes = 0;
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
                            smtpExceptionTimes++;
                            Logger.LogWarning(ex, $"寄送郵件 發生例外異常 ({smtpExceptionTimes})");
                        }

                        #region 超出最大嘗試送出 SMTP 郵件次數，要結束發信背景服務
                        if (smtpExceptionTimes > MagicHelper.MaxSmtpRetryTimes)
                        {
                            string message = $"連續嘗試 {MagicHelper.MaxSmtpRetryTimes} 次，無法送出 SMTP 郵件";
                            Logger.LogError(message);
                            SystemLogHelper SystemLogHelper = scope.ServiceProvider
                            .GetRequiredService<SystemLogHelper>();
                            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                            {
                                Message = message,
                                Category = LogCategories.SMTP,
                                Content = "",
                                LogLevel = LogLevels.Error,
                                Updatetime = DateTime.Now,
                                IP = "",
                            });
                            //cancellationTokenSource.Cancel();
                            smtpExceptionTimes = 0;
                        }
                        #endregion

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
            Logger.LogInformation($"寄送郵件 服務即將停止，共花費 {timeSpan}");

            return;
        }
    }
}
