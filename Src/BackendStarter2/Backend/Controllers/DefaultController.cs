﻿using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello~~";
        }
    }
}
