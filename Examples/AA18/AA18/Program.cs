using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AA18
{
    /// <summary>
    /// Web API 送出不同資料之 HttpClient 作法 5 (圖片檔
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/SendData/ImageFile";

                HttpResponseMessage response = await client.GetAsync(endPoint);
                string ImgFilePath = $"MyImage.png";
                ImgFilePath = Path.Combine(Environment.CurrentDirectory, ImgFilePath);
                using (var filestream = File.Open(ImgFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.CopyTo(filestream);
                    }
                }
                Console.WriteLine($"{ImgFilePath} 已經下載成功");
            }
        }
    }
}
