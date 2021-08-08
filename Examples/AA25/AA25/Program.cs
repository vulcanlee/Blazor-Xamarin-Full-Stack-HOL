using AA25.ApiServices;
using AA25.CommonApiServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AA25
{
    /// <summary>
    /// 使用 BaseWebAPI 設計 Product CRUD 存取服務
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            APIResult result;
            LoginService loginService = new LoginService();
            result = await loginService.PostAsync(new CommonApiServices.LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            Console.WriteLine($"存取權杖 : {loginService.SingleItem.Token}");
            ProductService productService = new ProductService();

            #region Retrive 查詢
            Console.WriteLine($"\n查詢所有產品紀錄 (Retrive)");
            result = await productService.GetAsync();
            Console.WriteLine($"全部產品共有 {productService.Items.Count} 筆記錄");
            #endregion

            #region Create 新增
            Console.WriteLine($"\n新增產品 A (Create)");
            result = await productService.PostAsync(new ProductDto()
            {
                Name = $"練習產品 A {DateTime.Now.Ticks}",
                ListPrice = 300,
                ModelYear = 2022,
            });
            result = await productService.GetAsync();
            Console.WriteLine($"新增產品 B (Create)");
            result = await productService.PostAsync(new ProductDto()
            {
                Name = $"練習產品 B {DateTime.Now.Ticks}",
                ListPrice = 300,
                ModelYear = 2022,
            });
            result = await productService.GetAsync();
            Console.WriteLine($"全部產品共有 {productService.Items.Count} 筆記錄");
            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
            #endregion

            #region Update 更新
            Console.WriteLine($"\n更新產品 A (Update)");
            result = await productService.GetAsync();
            var updateItem = productService.Items.FirstOrDefault(x => x.Name.Contains("練習產品 A"));
            updateItem.ListPrice = 23172;
            updateItem.ModelYear = 2054;
            result = await productService.PutAsync(updateItem);
            result = await productService.GetAsync();
            Console.WriteLine($"全部產品共有 {productService.Items.Count} 筆記錄");
            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
            #endregion

            #region Delete 刪除
            Console.WriteLine($"\n刪除產品 B (Delete)");
            result = await productService.GetAsync();
            var deleteItem = productService.Items.FirstOrDefault(x => x.Name.Contains("練習產品 B"));
            result = await productService.DeleteAsync(deleteItem);
            result = await productService.GetAsync();
            Console.WriteLine($"全部產品共有 {productService.Items.Count} 筆記錄");
            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();

            #endregion
        }
    }
}
