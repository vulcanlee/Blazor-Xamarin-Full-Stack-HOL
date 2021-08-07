using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AA20
{
    /// <summary>
    /// HttpClient 使用 取消權杖 的程式設計方法
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.KeyChar == 'c')
                    cts.Cancel();
            });
            Console.WriteLine($"開始呼叫 Web API，按下按鍵 c ，則取消此非同步作業");
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string endPoint = $"https://localhost:5001/api/DoAsyncWork/15000";
                    CancellationToken token = cts.Token;
                    HttpResponseMessage response = await client.GetAsync(endPoint, token);
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"此次呼叫取得狀態碼 {response.StatusCode} ({(int)response.StatusCode})");
                    Console.WriteLine($"此次呼叫取得內容 {result}");
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine($"非同步作業已經取消了 : {ex.Message}");
                }
            }

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }
    }
}
