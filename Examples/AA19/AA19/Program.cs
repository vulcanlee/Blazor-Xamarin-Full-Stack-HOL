using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA19
{
    /// <summary>
    /// HttpClient 送出不同資料作法 8 (圖片檔)
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
                string endPoint = $"https://localhost:5001/api/Upload";
                HttpResponseMessage response = null;

                #region 將剛剛拍照的檔案，上傳到網路伺服器上(使用 Multipart 的規範)
                // 規格說明請參考 https://www.w3.org/Protocols/rfc1341/7_2_Multipart.html
                using (var content = new MultipartFormDataContent())
                {
                    var filename = "Blazor.png";
                    content.Add(new StringContent(filename), "description");
                    var rootPath = Directory.GetCurrentDirectory();
                    // 取得這個圖片檔案的完整路徑
                    var path = Path.Combine(rootPath, filename);

                    // 開啟這個圖片檔案，並且讀取其內容
                    using (var fs = File.Open(path, FileMode.Open))
                    {
                        var streamContent = new StreamContent(fs);
                        streamContent.Headers.Add("Content-Type", "application/octet-stream");
                        streamContent.Headers.Add("Content-Disposition",
                            "form-data; name=\"file\"; filename=\"" + filename + "\"");
                        content.Add(streamContent, "file", filename);

                        // 上傳到遠端伺服器上
                        response = await client.PostAsync(endPoint, content);
                    }
                }
                #endregion
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
