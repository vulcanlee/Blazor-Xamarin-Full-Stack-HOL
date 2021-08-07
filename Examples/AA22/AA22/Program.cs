using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA22
{
    /// <summary>
    /// HttpClient 讀取不同狀態碼與訊息內容
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                //string endPoint = $"https://localhost:5001/api/SendStatus/Ok";
                //string endPoint = $"https://localhost:5001/api/SendStatus/NoContent";
                //string endPoint = $"https://localhost:5001/api/SendStatus/NotFound";
                //string endPoint = $"https://localhost:5001/api/SendStatus/BadRequest";
                //string endPoint = $"https://localhost:5001/api/SendStatus/Unauthorized";
                string endPoint = $"https://localhost:5001/api/SendStatus/CustomStausCode";

                HttpResponseMessage response = await client.GetAsync(endPoint);
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"此次呼叫取得狀態碼 {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"此次呼叫取得內容 {result}");
            }

        }
    }
}
