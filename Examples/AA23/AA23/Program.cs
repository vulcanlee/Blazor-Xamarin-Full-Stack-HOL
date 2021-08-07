using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA23
{
    /// <summary>
    /// HttpClient 直接讀取回應字串
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

                var foo = await client.GetStringAsync(endPoint);
                Console.WriteLine(foo);
            }
        }
    }
}
