using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA04
{
    /// <summary>
    /// HttpClient 送出不同資料作法 2(Multipart/Form Data)
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

                #region 使用 MultipartFormDataContent (form-data) 產生要 Post 的資料
                //https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/nameof
                // 準備要 Post 的資料
                Dictionary<string, string> formDataDictionary = new Dictionary<string, string>()
                {
                    {nameof(loginRequestDto.Account), loginRequestDto.Account.ToString() },
                    {nameof(loginRequestDto.Password), loginRequestDto.Password },
                };

                #endregion

                // https://msdn.microsoft.com/zh-tw/library/system.net.http.multipartformdatacontent(v=vs.110).aspx
                using (var fooContent = new MultipartFormDataContent())
                {
                    foreach (var keyValuePair in formDataDictionary)
                    {
                        fooContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }
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
