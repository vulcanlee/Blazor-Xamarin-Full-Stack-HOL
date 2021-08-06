using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public GetDataController(IWebHostEnvironment webHostEnvironment)
        {
            this.environment = webHostEnvironment;
        }
        [HttpGet("Cookie")]
        public async Task<IActionResult> GetCookie()
        {
            await Task.Yield();
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                IsEssential = true, //<- there
                Expires = DateTime.Now.AddMonths(1),
                Secure = true,
            };
            HttpContext.Response.Cookies.Append("YourName", "Vulcan", cookieOptions);

            return Ok("Cookie");
        }
        [HttpGet("Header")]
        public async Task<IActionResult> GetHeader()
        {
            await Task.Yield();

            Response.Headers.Add("X-Total-Count", "20");

            return Ok("Header");
        }
        [HttpGet("Object")]
        public async Task<IActionResult> GetObject()
        {
            await Task.Yield();

            LoginRequestDto loginRequestDto = new()
            {
                Account = "Vulcan",
                Password = "Lee",
            };

            return Ok(loginRequestDto);
        }
        [HttpGet("File")]
        public async Task<IActionResult> GetFile()
        {
            await Task.Yield();

            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;
            string fileName = "site.css";
            string filePath = Path.Combine(wwwPath, "css", fileName);
            // https://developer.mozilla.org/zh-TW/docs/Web/HTTP/Basics_of_HTTP/MIME_types#%E9%87%8D%E8%A6%81%E7%9A%84mime%E9%A1%9E%E5%88%A5
            string contentType = "text/css";
            PhysicalFileResult fileResult = new PhysicalFileResult(filePath, contentType);

            return fileResult;
        }
    }
}
