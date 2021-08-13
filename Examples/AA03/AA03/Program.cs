using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA03
{
    /// <summary>
    /// HttpClient 送出不同資料作法 1 (Form UrlEncoded)
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
                string endPoint = $"https://localhost:5001/api/GetParameter/FromForm";
                HttpResponseMessage response = null;

                #region 使用 FormUrlEncodedContent (x-www-form-urlencoded) 產生要 Post 的資料
                //// 方法一： 使用字串名稱用法
                //var formData = new FormUrlEncodedContent(new[] {
                //    new KeyValuePair<string, string>(nameof(loginRequestDto.Account), loginRequestDto.Account.ToString()),
                //    new KeyValuePair<string, string>(nameof(loginRequestDto.Password), loginRequestDto.Password),
                //});

                // 方法二： 強型別用法
                // https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/nameof
                Dictionary<string, string> formDataDictionary = new Dictionary<string, string>()
                {
                    {nameof(loginRequestDto.Account), loginRequestDto.Account.ToString() },
                    {nameof(loginRequestDto.Password), loginRequestDto.Password.ToString() },
                };

                #endregion       
                
                // https://msdn.microsoft.com/zh-tw/library/system.net.http.formurlencodedcontent(v=vs.110).aspx
                using (var fooContent = new FormUrlEncodedContent(formDataDictionary))
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
