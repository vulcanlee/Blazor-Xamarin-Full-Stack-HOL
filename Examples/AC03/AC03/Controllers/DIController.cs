using AC03.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace AC03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DIController : ControllerBase
    {
        private readonly IMyServiceTransient myServiceTransient1;
        private readonly IMyServiceTransient myServiceTransient2;
        private readonly IMyServiceScoped myServiceScoped1;
        private readonly IMyServiceScoped myServiceScoped2;
        private readonly IMyServiceSingleton myServiceSingleton1;
        private readonly IMyServiceSingleton myServiceSingleton2;
        private readonly MyService myService;

        public DIController(IMyServiceTransient myServiceTransient1, IMyServiceTransient myServiceTransient2,
            IMyServiceScoped myServiceScoped1, IMyServiceScoped myServiceScoped2,
            IMyServiceSingleton myServiceSingleton1, IMyServiceSingleton myServiceSingleton2, MyService myService)
        {
            this.myServiceTransient1 = myServiceTransient1;
            this.myServiceTransient2 = myServiceTransient2;
            this.myServiceScoped1 = myServiceScoped1;
            this.myServiceScoped2 = myServiceScoped2;
            this.myServiceSingleton1 = myServiceSingleton1;
            this.myServiceSingleton2 = myServiceSingleton2;
            this.myService = myService;
        }

        [HttpGet]
        public string Get()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"注入 [類別] 暫時性物件1 " + Environment.NewLine);
            result.Append($"Guid:{myService.Guid} , HashCode :{myService.HashCode}" +
                Environment.NewLine);
            result.Append($"注入暫時性物件1 " + Environment.NewLine);
            result.Append($"Guid:{myServiceTransient1.Guid} , HashCode :{myServiceTransient1.HashCode}" +
                Environment.NewLine);
            result.Append($"注入暫時性物件2 " + Environment.NewLine);
            result.Append($"Guid:{myServiceTransient2.Guid} , HashCode :{myServiceTransient2.HashCode}" +
                Environment.NewLine + Environment.NewLine);
            result.Append($"注入範圍性物件1 " + Environment.NewLine);
            result.Append($"Guid:{myServiceScoped1.Guid} , HashCode :{myServiceScoped1.HashCode}" +
                Environment.NewLine);
            result.Append($"注入範圍性物件2 " + Environment.NewLine);
            result.Append($"Guid:{myServiceScoped2.Guid} , HashCode :{myServiceScoped2.HashCode}" +
                Environment.NewLine + Environment.NewLine);
            result.Append($"注入永久性物件1 " + Environment.NewLine);
            result.Append($"Guid:{myServiceSingleton1.Guid} , HashCode :{myServiceSingleton1.HashCode}" +
                Environment.NewLine);
            result.Append($"注入永久性物件2 " + Environment.NewLine);
            result.Append($"Guid:{myServiceSingleton2.Guid} , HashCode :{myServiceSingleton2.HashCode}" +
                Environment.NewLine);

            return result.ToString();
        }
    }
}
