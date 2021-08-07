using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA14
{
    // Web API 送出不同資料之 HttpClient 作法 1 (Body)
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                //string endPoint = $"https://localhost:5001/api/SendData/String";
                //string endPoint = $"https://localhost:5001/api/SendData/StringAsync";
                //string endPoint = $"https://localhost:5001/api/SendData/Object";
                string endPoint = $"https://localhost:5001/api/SendData/ObjectWithStatus";

                HttpResponseMessage response = await client.GetAsync(endPoint);
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"此次呼叫取得內容 {result}");
            }
        }
    }
}
