using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AA02
{
    class Program
    {
        static async Task Main(string[] args)
        {
            (APIResult apiResult, LoginResponseDto response) = await LoginPostAsync(new LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            Console.WriteLine($"存取權杖 : {response.Token}");
            Console.WriteLine($"");
            Console.WriteLine($"更新權杖 : {response.RefreshToken}");
        }
        private static async Task<(APIResult, LoginResponseDto)> LoginPostAsync(LoginRequestDto loginRequestDto)
        {
            APIResult apiResult;
            LoginResponseDto loginResponseDto=null;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    #region 準備開啟連線
                    string endPoint = $"https://localhost:5001/api/login";
                    HttpResponseMessage response = null;

                    // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                    //client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Content-Type 用於宣告遞送給對方的文件型態
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var JsonPayload = JsonConvert.SerializeObject(loginRequestDto);
                    // https://msdn.microsoft.com/zh-tw/library/system.net.http.stringcontent(v=vs.110).aspx
                    using (var fooContent = new StringContent(JsonPayload, Encoding.UTF8, "application/json"))
                    {
                        response = await client.PostAsync(endPoint, fooContent);
                    }
                    #endregion

                    #region 處理呼叫完成 Web API 之後的回報結果
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();
                            apiResult = JsonConvert.DeserializeObject<APIResult>(strResult,
                                new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(apiResult.Payload.ToString(), 
                                new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                        }
                        else
                        {
                            apiResult = new APIResult
                            {
                                Status = false,
                                Message = string.Format("Error Code:{0}, Error Message:{1}", response.StatusCode, response.RequestMessage),
                                Payload = null,
                            };
                        }
                    }
                    else
                    {
                        apiResult = new APIResult
                        {
                            Status = false,
                            Message = $"應用程式呼叫 API ({endPoint}) 發生異常",
                            Payload = null,
                        };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    apiResult = new APIResult
                    {
                        Status = false,
                        Message = ex.Message,
                        Payload = ex,
                    };
                }
            }

            return (apiResult, loginResponseDto);
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
