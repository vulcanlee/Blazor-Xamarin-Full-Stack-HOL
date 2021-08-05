using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AA01
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
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
        private static async Task<APIResult> LoginPostAsync(LoginRequestDto loginRequestDto)
        {
            APIResult fooAPIResult;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/login";
                HttpResponseMessage response = null;

                var JsonPayload = JsonConvert.SerializeObject(loginRequestDto);
                using (var fooContent = new StringContent(JsonPayload, Encoding.UTF8, "application/json"))
                {
                    response = await client.PostAsync(endPoint, fooContent);
                }

                String strResult = await response.Content.ReadAsStringAsync();
                fooAPIResult = JsonConvert.DeserializeObject<APIResult>(strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
            }

            return fooAPIResult;
        }
    }

    #region 呼叫 Web API 會用到的類別
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public bool Status { get; set; } = true;
        public int HTTPStatus { get; set; } = 200;
        public int ErrorCode { get; set; }
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object Payload { get; set; }
    }
    public class LoginRequestDto
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public int TokenExpireMinutes { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }
    #endregion
}
