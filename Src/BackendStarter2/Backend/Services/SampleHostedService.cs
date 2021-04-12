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
    public class SampleHostedService : IHostedService
    {
        public SampleHostedService(ILogger<SampleHostedService> logger,
            IServer server, IConfiguration configuration)
        {
            Logger = logger;
            Server = server;
            Configuration = configuration;
        }

        public ILogger<SampleHostedService> Logger { get; }
        public IServer Server { get; }
        public IConfiguration Configuration { get; }

        DateTime lastLogTime;
        int keepAliveCycle = 300;
        int checkCycle = 3;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Task.Factory.StartNew(async () =>
                {
                    lastLogTime = DateTime.Now;
                    while (cancellationToken.IsCancellationRequested==false)
                    {
                        var nextTime = lastLogTime.AddSeconds(keepAliveCycle);
                        if(DateTime.Now> nextTime)
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
                            Logger.LogInformation($"Keep alive {address}: {dateOffset.ToString()}");

                            try
                            {
                                var client =new HttpClient();
                                await client.GetStringAsync($"{address}/Login");
                            }
                            catch { }
                            lastLogTime = DateTime.Now;
                        }
                        await Task.Delay(checkCycle * 1000, cancellationToken);
                    }
                }, TaskCreationOptions.LongRunning);
            }
            catch { }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
