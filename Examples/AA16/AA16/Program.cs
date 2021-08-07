using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AA16
{
    /// <summary>
    /// Web API 送出不同資料之 HttpClient 作法 3 (Header)
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/SendData/Header";

                HttpResponseMessage response = await client.GetAsync(endPoint);
                result = await response.Content.ReadAsStringAsync();

                HttpHeaders headers = response.Headers;
                IEnumerable<string> values;
                string totalCount = "";
                if (headers.TryGetValues("X-Total-Count", out values))
                {
                    totalCount = values.First();
                }
                Console.WriteLine($"此次呼叫取得內容 {result}");
                Console.WriteLine($"X-Total-Count : {totalCount}");
            }
        }
    }
}
