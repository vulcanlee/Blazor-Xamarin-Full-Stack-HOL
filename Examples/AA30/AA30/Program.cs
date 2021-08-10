using RestSharp;
using System;
using System.Threading.Tasks;

namespace AA30
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new RestClient($"https://localhost:5001");

            var request = new RestRequest("/api/SendStatus/CustomStausCode", Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine($"此次呼叫取得狀態碼 {response.StatusCode} ({(int)response.StatusCode})");
            Console.WriteLine($"此次呼叫取得內容 {response.Content}");
        }
    }
}
