using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBusiness.Helpers;

namespace Backend.Controllers
{
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "Administrator")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OnlyAdministratorController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello Administrator~~";
        }
    }
}
