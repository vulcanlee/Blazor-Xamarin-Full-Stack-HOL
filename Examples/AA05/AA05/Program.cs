using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AA05
{
    /// <summary>
    /// HttpClient 送出不同資料作法 3 (JSON 編碼)
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await LoginAsync(new LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            Console.WriteLine(result);
        }
        private static async Task<string> LoginAsync(LoginRequestDto loginRequestDto)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/PassParameter/FromBody";
                HttpResponseMessage response = null;

                #region 使用 JSON (Raw > JSON) 產生要 Post 的資料
                // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                //client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Content-Type 用於宣告遞送給對方的文件型態
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var fooJSON = JsonConvert.SerializeObject(loginRequestDto);
                #endregion

                // https://msdn.microsoft.com/zh-tw/library/system.net.http.stringcontent(v=vs.110).aspx
                using (var fooContent = new StringContent(fooJSON, Encoding.UTF8, "application/json"))
                {
                    response = await client.PostAsync(endPoint, fooContent);
                }

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
    }

    #region 呼叫 Web API 會用到的類別
    public class LoginRequestDto
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
    }
    #endregion
}
