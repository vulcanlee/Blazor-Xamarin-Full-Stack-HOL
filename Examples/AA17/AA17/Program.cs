using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA17
{
    /// <summary>
    /// Web API 送出不同資料之 HttpClient 作法 4 (文字檔)
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/SendData/CssFile";

                HttpResponseMessage response = await client.GetAsync(endPoint);
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"此次呼叫取得內容 {result}");
            }
        }
    }
}
