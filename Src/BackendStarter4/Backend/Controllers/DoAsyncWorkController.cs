using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoAsyncWorkController : ControllerBase
    {
        [HttpGet("{waitPeriodOfTime}")]
        public async Task<string> GetAsync([FromRoute] int waitPeriodOfTime)
        {
            await Task.Delay(waitPeriodOfTime);
            System.Console.WriteLine($" DoAsyncWork 將要完成");
            return "Hello~~";
        }
    }
}
