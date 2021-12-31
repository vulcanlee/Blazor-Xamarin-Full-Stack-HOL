using ClientApi.ApiServices;
using ClientApi.CommonApiServices;
using DTOs.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApiSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LoginService loginService = new LoginService();
            var apiResult = await loginService.PostAsync(new LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            apiResult = await ProductRetriveAsync();
            apiResult = await ProductCreateAsync(new ProductDto()
            {
                Name = $"產品範例紀錄名稱 {DateTime.Now.Ticks}",
                ListPrice = 168,
                ModelYear = 2054,
            });
            apiResult = await ProductRetriveAsync();
            apiResult = await ProductUpdateAsync();
            apiResult = await ProductRetriveAsync();
            apiResult = await ProductDeleteAsync();
            apiResult = await ProductRetriveAsync();
        }

        #region CRUD Retrive 查詢
        private static async Task<APIResult> ProductRetriveAsync()
        {
            LoginService loginService = new LoginService();
            await loginService.ReadFromFileAsync();
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            ProductService productService = new ProductService();
            var apiResult = await productService.GetAsync();
            Console.WriteLine($"列出所有產品清單");
            foreach (var item in productService.Items)
            {
                Console.WriteLine($"{item.Name}");
            }
            Console.WriteLine();
            return apiResult;
        }
        #endregion

        #region CRUD Create 新增
        private static async Task<APIResult> ProductCreateAsync(ProductDto productDto)
        {
            LoginService loginService = new LoginService();
            await loginService.ReadFromFileAsync();
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            ProductService productService = new ProductService();
            var apiResult = await productService.PostAsync(productDto);
            Console.WriteLine($"新增 產品範例紀錄");
            Console.WriteLine();
            return apiResult;
        }
        #endregion

        #region CRUD Update 更新
        private static async Task<APIResult> ProductUpdateAsync()
        {
            LoginService loginService = new LoginService();
            await loginService.ReadFromFileAsync();
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            ProductService productService = new ProductService();
            await productService.ReadFromFileAsync();
            ProductDto productDto = productService.Items.LastOrDefault();
            productDto.Name = $"促銷而變更產品名稱";
            var apiResult = await productService.PutAsync(productDto);
            Console.WriteLine($"更新 產品範例紀錄");
            Console.WriteLine();
            return apiResult;
        }
        #endregion

        #region CRUD Delete 刪除
        private static async Task<APIResult> ProductDeleteAsync()
        {
            LoginService loginService = new LoginService();
            await loginService.ReadFromFileAsync();
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            ProductService productService = new ProductService();
            await productService.ReadFromFileAsync();
            ProductDto productDto = productService.Items.LastOrDefault();
            var apiResult = await productService.DeleteAsync(productDto);
            Console.WriteLine($"刪除 產品範例紀錄");
            Console.WriteLine();
            return apiResult;
        }
        #endregion

    }
}
