using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareBusiness.Factories;
using ShareBusiness.Helpers;
using ShareDomain.Enums;

namespace Backend.Controllers
{
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OnlyUserController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(APIResultFactory.Build(true, StatusCodes.Status201Created,
                        ErrorMessageEnum.None, payload: new OnlyUserDto()
                        { Message = "Hello User~~" }));
        }
    }
}
