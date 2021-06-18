using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AC07.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebLogController : ControllerBase
    {
        public WebLogController(ILogger<WebLogController> logger)
        {
            Logger = logger;
        }

        public ILogger<WebLogController> Logger { get; }

        [HttpGet]
        public async Task<string> Get()
        {
            StringBuilder result = new StringBuilder();

            var client = new HttpClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                
                Logger.LogDebug($"開始讀取 Google 網頁資料");
                string message = await client.GetStringAsync("http://www.google.com");
                Logger.LogInformation($"完成讀取 Google 網頁資料");
                stopwatch.Stop();
                result.Append($"讀取 google 資料共花費 {stopwatch.ElapsedMilliseconds} ms" + Environment.NewLine);
                
                Logger.LogInformation($"開始讀取 m1cr050ft 網頁資料");
                message = await client.GetStringAsync("http://www.m1cr050ft.com");
                Logger.LogInformation($"完成讀取 m1cr050ft 網頁資料");
                result.Append($"讀取 micr0s0ft 資料共花費 {stopwatch.ElapsedMilliseconds} ms" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"使用 HttpClient 發生了問題");
            }

            return result.ToString();
        }
    }
}
