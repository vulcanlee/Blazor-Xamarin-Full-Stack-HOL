using AA11.ApiServices;
using AA11.CommonApiServices;
using System;
using System.Threading.Tasks;

namespace AA11
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LoginService loginService = new LoginService();
            var result = await loginService.PostAsync(new CommonApiServices.LoginRequestDto()
            {
                Account = "user1",
                Password = "123"
            });
            LOBGlobal.AccessToken = loginService.SingleItem.Token;
            Console.WriteLine($"存取權杖 : {loginService.SingleItem.Token}");
            OnlyUserService onlyUserService = new OnlyUserService();
            result = await onlyUserService.GetAsync();
            Console.WriteLine($"呼叫結果 : {onlyUserService.SingleItem.Message}");
        }
    }
}
