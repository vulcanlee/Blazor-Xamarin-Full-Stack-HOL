using RestSharp;
using System;
using System.Threading.Tasks;

namespace AA31
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new RestClient($"https://localhost:5001");

            //var request = new RestRequest("/api/SendStatus/OK", Method.GET);
            //var request = new RestRequest("/api/SendStatus/NotFound", Method.GET);
            //var request = new RestRequest("/api/SendStatus/BadRequest", Method.GET);
            //var request = new RestRequest("/api/SendStatus/Unauthorized", Method.GET);
            var request = new RestRequest("/api/SendStatus/CustomStausCode", Method.GET);

            var response = await client.GetAsync<string>(request);
            Console.WriteLine($"此次呼叫取得內容 {response}");
        }
    }
}
