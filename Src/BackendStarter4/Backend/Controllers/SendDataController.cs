using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
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
    public class SendDataController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public SendDataController(IWebHostEnvironment webHostEnvironment)
        {
            this.environment = webHostEnvironment;
        }

        [HttpGet("String")]
        public string GetString()
        {
            return $"Hi Vulcan Lee";
        }

        [HttpGet("StringAsync")]
        public async Task<string> GetStringAsync()
        {
            await Task.Yield();
            return $"Hi Vulcan Lee";
        }

        [HttpGet("Object")]
        public LoginRequestDto GetObject()
        {
            LoginRequestDto loginRequestDto = new()
            {
                Account = "Vulcan",
                Password = "Lee",
            };

            return loginRequestDto;
        }

        [HttpGet("ObjectWithStatus")]
        public IActionResult GetObjectWithStatus()
        {
            LoginRequestDto loginRequestDto = new()
            {
                Account = "Vulcan",
                Password = "Lee",
            };

            return Ok(loginRequestDto);
        }

        [HttpGet("Cookie")]
        public IActionResult GetCookie()
        {
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
        public IActionResult GetHeader()
        {
            Response.Headers.Add("X-Total-Count", "20");

            return Ok("Header");
        }

        [HttpGet("CssFile")]
        public IActionResult GetCssFile()
        {
            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;
            string fileName = "site.css";
            string filePath = Path.Combine(wwwPath, "css", fileName);
            // https://developer.mozilla.org/zh-TW/docs/Web/HTTP/Basics_of_HTTP/MIME_types#%E9%87%8D%E8%A6%81%E7%9A%84mime%E9%A1%9E%E5%88%A5
            string contentType = "text/css";
            PhysicalFileResult fileResult = new PhysicalFileResult(filePath, contentType);

            return fileResult;
        }

        [HttpGet("ImageFile")]
        public IActionResult GetImageFile()
        {
            string wwwPath = environment.WebRootPath;
            string contentPath = environment.ContentRootPath;
            string fileName = "Blazor.png";
            string filePath = Path.Combine(wwwPath, "Images", fileName);
            // https://developer.mozilla.org/zh-TW/docs/Web/HTTP/Basics_of_HTTP/MIME_types#%E9%87%8D%E8%A6%81%E7%9A%84mime%E9%A1%9E%E5%88%A5
            string contentType = "image/png";
            PhysicalFileResult fileResult = new PhysicalFileResult(filePath, contentType);

            return fileResult;
        }
    }
}
