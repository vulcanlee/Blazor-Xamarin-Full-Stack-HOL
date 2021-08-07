using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA12
{
    /// <summary>
    /// 沒有經過身分驗證，呼叫有授權才能存取的 API
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await OnlyUserGetAsync();
            Console.WriteLine(result.Message);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
        private static async Task<OnlyUserDto> OnlyUserGetAsync()
        {
            OnlyUserDto onlyUserDto;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/OnlyUser";
                HttpResponseMessage response = null;
                response = await client.GetAsync(endPoint);
                String strResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"狀態碼 : {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"取得結果 : {strResult}");
                onlyUserDto = JsonConvert.DeserializeObject<OnlyUserDto>(strResult,
                    new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
            }

            return onlyUserDto;
        }
    }

    #region 呼叫 Web API 會用到的類別
    public class OnlyUserDto
    {
        public string Message { get; set; }
    }
    #endregion
}
