using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HasExceptionController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            int foo = 100;
            foo = foo / 0;
            return "Hello~~";
        }
    }
}
