using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AA29
{
    /// <summary>
    /// HttpClient 呼叫 CRUDController 修改 API
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await DeleteAsync(new CRUDDto()
            {
                Id = 123,
                Name = "CRUD123",
                Price = 12300.0m,
                Updatetime = DateTime.Now,
            });
            Console.WriteLine(result);
        }
        private static async Task<string> DeleteAsync(CRUDDto dto)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/CRUD/{dto.Id}";
                HttpResponseMessage response = null;

                #region 使用 JSON (Raw > JSON) 產生要 Post 的資料
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                #endregion

                response = await client.DeleteAsync(endPoint);
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }

    #region 呼叫 Web API 會用到的類別
    public class CRUDDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 欄位必須要輸入值")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Updatetime { get; set; }
    }
    #endregion
}
