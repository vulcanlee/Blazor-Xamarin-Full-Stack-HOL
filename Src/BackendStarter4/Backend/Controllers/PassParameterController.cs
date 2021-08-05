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
        [AllowAnonymous]
        [Route("FromBody")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequestDto loginRequestDTO)
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
