using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController : ControllerBase
    {
        [HttpGet("Add/{Value1}/{Value2}/{Cost}")]
        public int Add([FromRoute] int value1, int value2, int cost)
        {
            int sum = value1 + value2;
            Thread.Sleep(cost);
            return sum;
        }

        [HttpGet("AddAsync/{Value1}/{Value2}/{Cost}")]
        public async Task<int> AddAsync([FromRoute] int value1, int value2, int cost)
        {
            int sum = value1 + value2;

            try
            {
                await Task.Delay(cost, HttpContext.RequestAborted);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return sum;
        }

        [HttpGet("Checksum/{Value}/{Cost}")]
        public string Checksum([FromRoute] int value, int cost)
        {
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(value.ToString());
            byte[] crypto = md5.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            Thread.Sleep(cost);
            return result;
        }

        [HttpGet("ChecksumAsync/{Value}/{Cost}")]
        public async Task<string> ChecksumAsync([FromRoute] int value, int cost)
        {
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(value.ToString());
            byte[] crypto = md5.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);

            try
            {
                await Task.Delay(cost, HttpContext.RequestAborted);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return result;
        }

    }
}
