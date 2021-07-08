using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SomethingController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetAsync()
        {
            await Task.Delay(3000);
            return "Hello~~";
        }
    }
}
