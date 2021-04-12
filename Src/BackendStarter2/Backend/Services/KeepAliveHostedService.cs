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
        int keepAliveCycle = 300;
        int checkCycle = 3;
        DateTime StartupTime = DateTime.Now;
        Task keepAliveTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            Logger.LogInformation($"Keep alive 服務開始啟動");
            keepAliveTask = Task.Factory.StartNew(async() =>
            {
                try
                {
                    StartupTime = DateTime.Now;
                    lastLogTime = DateTime.Now;
                    HttpClient client = new HttpClient();
                    while (cancellationToken.IsCancellationRequested == false)
                    {
                        var nextTime = lastLogTime.AddSeconds(keepAliveCycle);
                        if (DateTime.Now > nextTime)
                        {
                            //var features = Server.Features;
                            //var addresses = features.Get<IServerAddressesFeature>();
                            //foreach (var item in addresses.Addresses)
                            //{
                            //    Logger.LogInformation($"item {item}");
                            //}
                            //var address = addresses.Addresses.FirstOrDefault();

                            var address = Configuration["KeepAliveEndpoint"];
                            var dateOffset = DateTime.UtcNow.AddHours(8);
                            TimeSpan timeSpan = DateTime.Now - StartupTime;
                            Logger.LogInformation($"Keep alive ({timeSpan}) {dateOffset.ToString()}");

                            try
                            {
                                await client.GetStringAsync($"{address}/Login");
                            }
                            catch { }
                            lastLogTime = DateTime.Now;
                        }
                        await Task.Delay(checkCycle * 1000, cancellationToken);
                    }
                }
                catch { }
            },TaskCreationOptions.LongRunning);
            //new Thread(async x =>
            //{
            //    try
            //    {
            //        StartupTime = DateTime.Now;
            //        lastLogTime = DateTime.Now;
            //        HttpClient client = new HttpClient();
            //        while (cancellationToken.IsCancellationRequested == false)
            //        {
            //            var nextTime = lastLogTime.AddSeconds(keepAliveCycle);
            //            if (DateTime.Now > nextTime)
            //            {
            //                //var features = Server.Features;
            //                //var addresses = features.Get<IServerAddressesFeature>();
            //                //foreach (var item in addresses.Addresses)
            //                //{
            //                //    Logger.LogInformation($"item {item}");
            //                //}
            //                //var address = addresses.Addresses.FirstOrDefault();

            //                var address = Configuration["KeepAliveEndpoint"];
            //                var dateOffset = DateTime.UtcNow.AddHours(8);
            //                TimeSpan timeSpan = DateTime.Now - StartupTime;
            //                Logger.LogInformation($"Keep alive ({timeSpan}) {dateOffset.ToString()}");

            //                try
            //                {
            //                    await client.GetStringAsync($"{address}/Login");
            //                }
            //                catch { }
            //                lastLogTime = DateTime.Now;
            //            }
            //            await Task.Delay(checkCycle * 1000, cancellationToken);
            //        }
            //    }
            //    catch { }
            //}).Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TimeSpan timeSpan = DateTime.Now - StartupTime;
            Logger.LogInformation($"Keep alive 服務即將停止，共花費 {timeSpan}");

            return Task.FromResult(0);
        }
    }
}
