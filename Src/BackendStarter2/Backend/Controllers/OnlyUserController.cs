using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareBusiness.Factories;
using ShareBusiness.Helpers;
using ShareDomain.Enums;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    /// <summary>
    /// 需要使用具有 一般使用者 權限帳號者，才能夠呼叫
    /// </summary>
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OnlyUserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(TimeSpan.FromSeconds(20));
            return Ok(APIResultFactory.Build(true, StatusCodes.Status201Created,
                        ErrorMessageEnum.None, payload: new OnlyUserDto()
                        { Message = "Hello User~~" }));
        }
    }
}
