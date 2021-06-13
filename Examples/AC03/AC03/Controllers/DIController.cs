using AC03.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DIController(IMyServiceTransient myServiceTransient1, IMyServiceTransient myServiceTransient2,
            IMyServiceScoped myServiceScoped1, IMyServiceScoped myServiceScoped2,
            IMyServiceSingleton myServiceSingleton1, IMyServiceSingleton myServiceSingleton2)
        {
            this.myServiceTransient1 = myServiceTransient1;
            this.myServiceTransient2 = myServiceTransient2;
            this.myServiceScoped1 = myServiceScoped1;
            this.myServiceScoped2 = myServiceScoped2;
            this.myServiceSingleton1 = myServiceSingleton1;
            this.myServiceSingleton2 = myServiceSingleton2;
        }

        [HttpGet]
        public string Get()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"myServiceTransient1 " +
                $"Guid:{myServiceTransient1.Guid} , HashCode :{myServiceTransient1.HashCode}"+
                Environment.NewLine);
            result.Append($"myServiceTransient2 " +
                $"Guid:{myServiceTransient2.Guid} , HashCode :{myServiceTransient2.HashCode}" +
                Environment.NewLine);
            result.Append($"myServiceScoped1 " +
                $"Guid:{myServiceScoped1.Guid} , HashCode :{myServiceScoped1.HashCode}" +
                Environment.NewLine);
            result.Append($"myServiceScoped2 " +
                $"Guid:{myServiceScoped2.Guid} , HashCode :{myServiceScoped2.HashCode}" +
                Environment.NewLine);
            result.Append($"myServiceSingleton1 " +
                $"Guid:{myServiceSingleton1.Guid} , HashCode :{myServiceSingleton1.HashCode}" +
                Environment.NewLine);
            result.Append($"myServiceSingleton2 " +
                $"Guid:{myServiceSingleton2.Guid} , HashCode :{myServiceSingleton2.HashCode}" +
                Environment.NewLine);

            return result.ToString();
        }
}
}
