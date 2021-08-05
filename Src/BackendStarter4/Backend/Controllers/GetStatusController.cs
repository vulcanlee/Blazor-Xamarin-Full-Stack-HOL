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
    public class GetStatusController : ControllerBase
    {
        [HttpGet("OK")]
        public IActionResult GetOKAsync()
        {
            return Ok($"你得到 Ok 狀態");
        }
        [HttpGet("NotFound")]
        public IActionResult GetNotFound()
        {
            return NotFound($"你得到 NotFound 狀態");
        }
        [HttpGet("BadRequest")]
        public IActionResult GetBadRequestFound()
        {
            return BadRequest($"你得到 BadRequest 狀態");
        }
        [HttpGet("Unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized($"你得到 Unauthorized 狀態");
        }
    }
}
