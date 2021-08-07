using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AA21
{
    /// <summary>
    /// 使用 取消權杖 的 時間逾期 程式設計方法
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.WriteLine($"經過 5000 毫秒後，則取消此非同步作業");
            cts.CancelAfter(5000);
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
