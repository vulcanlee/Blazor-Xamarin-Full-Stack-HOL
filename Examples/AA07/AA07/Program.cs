using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA07
{
    /// <summary>
    /// HttpClient 送出不同資料作法 5 (路由)
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
                string endPoint = $"https://localhost:5001/api/GetParameter/FromRoute";
                HttpResponseMessage response = null;

                #region 使用 Route 資料
                endPoint = $"{endPoint}/{loginRequestDto.Account}/{loginRequestDto.Password}";
                #endregion

                response = await client.GetAsync(endPoint);

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
