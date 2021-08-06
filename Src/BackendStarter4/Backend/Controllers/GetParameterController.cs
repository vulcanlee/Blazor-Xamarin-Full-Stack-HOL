using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetParameterController : ControllerBase
    {
        [HttpGet("FromRoute/{Account}/{Password}")]
        public IActionResult GetFromRoute([FromRoute] LoginRequestDto loginRequestDTO)
        {
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [Route("FromQuery")]
        [HttpGet]
        public IActionResult GetFromQuery([FromQuery] LoginRequestDto loginRequestDTO)
        {
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [Route("FromBody")]
        [HttpPost]
        public IActionResult PostFromBody([FromBody] LoginRequestDto loginRequestDTO)
        {
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [HttpPost]
        [Route("FromForm")]
        public IActionResult PostFromForm([FromForm] LoginRequestDto loginRequestDTO)
        {
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [HttpPost]
        [Route("ByCookie")]
        public IActionResult PostByCookie()
        {
            string cookieValueFromReq = Request.Cookies["WhoAreYou"];
            return Ok($"Nice to meet you, {cookieValueFromReq}");
        }

        [HttpPost]
        [Route("ByHeader")]
        public IActionResult PostByHeader()
        {
            string cookieValueFromReq = Request.Headers["WhoAreYou"];
            return Ok($"Nice to meet you, {cookieValueFromReq}");
        }
    }
}
