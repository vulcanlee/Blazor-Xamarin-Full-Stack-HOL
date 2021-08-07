using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA06
{
    /// <summary>
    /// HttpClient 送出不同資料作法 4 (Query String)
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
                string endPoint = $"https://localhost:5001/api/PassParameter/FromQuery";
                HttpResponseMessage response = null;

                #region 使用 QueryString
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(loginRequestDto.Account), loginRequestDto.Account);
                dic.Add(nameof(loginRequestDto.Password), loginRequestDto.Password);

                string queryString = "?";
                foreach (string key in dic.Keys)
                {
                    queryString += key + "=" + dic[key] + "&";
                }
                queryString = queryString.Remove(queryString.Length - 1, 1);
                #endregion

                response = await client.GetAsync(endPoint + queryString);
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
}
    #endregion
