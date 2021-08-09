﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AA28
{
    /// <summary>
    /// HttpClient 呼叫 CRUDController 修改 API
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await PutAsync(new CRUDDto()
            {
                Id = 123,
                Name = "CRUD123",
                Price = 12300.0m,
                Updatetime = DateTime.Now,
            });
            Console.WriteLine(result);
        }
        private static async Task<string> PutAsync(CRUDDto dto)
        {
            string result = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = $"https://localhost:5001/api/CRUD/{dto.Id}";
                HttpResponseMessage response = null;

                #region 使用 JSON (Raw > JSON) 產生要 Post 的資料
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var fooJSON = JsonConvert.SerializeObject(dto);
                #endregion

                using (var fooContent = new StringContent(fooJSON, Encoding.UTF8, "application/json"))
                    response = await client.PutAsync(endPoint, fooContent);

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
