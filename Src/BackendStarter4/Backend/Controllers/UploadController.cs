using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            this.environment = webHostEnvironment;
        }
        [HttpPost]
        public IActionResult PostAsync(IFormFile file, [FromForm] string description)
        {
            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;

            return Ok($"{file.FileName} 已經上傳成功");
        }
    }
}
