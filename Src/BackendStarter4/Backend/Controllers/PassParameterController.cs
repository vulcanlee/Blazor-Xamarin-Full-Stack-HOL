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
    public class PassParameterController : ControllerBase
    {
        [Route("FromRoute")]
        [HttpPost]
        public async Task<IActionResult> PostFromRoute([FromRoute] LoginRequestDto loginRequestDTO)
        {
            await Task.Yield();
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [Route("FromQuery")]
        [HttpGet]
        public async Task<IActionResult> GetFromQuery([FromQuery] LoginRequestDto loginRequestDTO)
        {
            await Task.Yield();
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [Route("FromBody")]
        [HttpPost]
        public async Task<IActionResult> PostFromBody([FromBody] LoginRequestDto loginRequestDTO)
        {
            await Task.Yield();
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

        [HttpPost]
        [Route("FromForm")]
        public async Task<IActionResult> PostFromForm([FromForm] LoginRequestDto loginRequestDTO)
        {
            await Task.Yield();
            loginRequestDTO.Account = $"Hi {loginRequestDTO.Account}";
            return Ok(loginRequestDTO);
        }

    }
}
