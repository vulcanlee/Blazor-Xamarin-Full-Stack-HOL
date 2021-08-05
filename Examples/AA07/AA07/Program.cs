using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA07
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await LoginPostAsync(new LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            Console.WriteLine(result);
        }
        private static async Task<string> LoginPostAsync(LoginRequestDto loginRequestDto)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/PassParameter/FromRoute";
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
