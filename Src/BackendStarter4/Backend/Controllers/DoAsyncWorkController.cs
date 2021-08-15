using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                await Task.Delay(waitPeriodOfTime, HttpContext.RequestAborted);
                System.Console.WriteLine($" DoAsyncWork 將要完成");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            return "Hello~~";
        }
    }
}
