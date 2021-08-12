using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA32
{
    /// <summary>
    /// HttpClient 讀取不同狀態碼與訊息內容 (對應 HTTP)
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                // 傳送要求時所使用之網際網路資源的統一資源識別元 (URI) 基底位址
                client.BaseAddress = new Uri("https://localhost:5001"); ;

                HttpRequestMessage request = 
                    new HttpRequestMessage(HttpMethod.Get, "api/SendStatus/Ok");

                HttpResponseMessage response = await client.SendAsync(request);
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"此次呼叫取得狀態碼 {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"此次呼叫取得內容 {result}");
            }
        }
    }
}
