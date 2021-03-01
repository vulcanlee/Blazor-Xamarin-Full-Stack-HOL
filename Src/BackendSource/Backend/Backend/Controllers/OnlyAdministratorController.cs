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
    /// <summary>
    /// 需要使用具有 管理者 權限帳號者，才能夠呼叫
    /// </summary>
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "Administrator")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OnlyAdministratorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(APIResultFactory.Build(true, StatusCodes.Status201Created,
                        ErrorMessageEnum.None, payload: new OnlyAdministratorDto()
                        { Message = "Hello Administrator~~" }));
        }
    }
}
