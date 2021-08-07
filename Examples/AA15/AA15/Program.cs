using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA15
{
    /// <summary>
    /// Web API 送出不同資料之 HttpClient 作法 2 (Cookie)
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    string endPoint = $"https://localhost:5001/api/SendData/Cookie";

                    HttpResponseMessage response = await client.GetAsync(endPoint);
                    result = await response.Content.ReadAsStringAsync();
                    IEnumerable<Cookie> responseCookies = handler.CookieContainer
                        .GetCookies(new Uri("https://localhost:5001")).Cast<Cookie>();
                    var cookie = responseCookies.FirstOrDefault(x => x.Name == "YourName");
                    Console.WriteLine($"此次呼叫取得內容 {result}");
                    Console.WriteLine($"{cookie.Name} : {cookie.Value}");
                }
            }
        }
    }
}
