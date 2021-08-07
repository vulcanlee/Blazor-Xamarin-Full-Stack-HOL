using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA08
{
    /// <summary>
    /// HttpClient 送出不同資料作法 6 (Cookie)
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
                string endPoint = $"https://localhost:5001/api/GetParameter/ByCookie";
                HttpResponseMessage response = null;

                #region 使用 Cookie 資料
                client.DefaultRequestHeaders.Add("Cookie", $"WhoAreYou=Vulcan");
                #endregion

                response = await client.PostAsync(endPoint, null);

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
