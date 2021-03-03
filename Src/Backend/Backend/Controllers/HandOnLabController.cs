using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HandOnLabController : ControllerBase
    {
        [HttpGet("Add/{value1}/{value2}/{cost}")]
        public string GetAdd([FromRoute] int value1, [FromRoute] int value2, [FromRoute] int cost)
        {
            Thread.Sleep(cost * 1000);
            return (value1 + value2).ToString();
        }        //[Route("Add")]
        [HttpGet("AddAsync/{value1}/{value2}/{cost}")]
        public async Task<string> GetAddAsync([FromRoute] int value1, [FromRoute] int value2, [FromRoute] int cost)
        {
            await Task.Delay(cost * 1000);
            return (value1 + value2).ToString();
        }
    }
}
