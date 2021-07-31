using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SomethingController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetAsync()
        {
            await Task.Delay(3000);
            return "Hello~~";
        }

        [Route("ThreadPool")]
        [HttpGet]
        public string ThreadPoolInformation()
        {
            string result = "";
            int workerThreads; int portThreads;
            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            result += $"ThreadPool Max : {workerThreads} / {portThreads}@";
            ThreadPool.GetMinThreads(out workerThreads, out portThreads);
            result += $"ThreadPool Min : {workerThreads} / {portThreads}@";
            ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
            result += $"ThreadPool Available : {workerThreads} / {portThreads}@";
            result += "@";
            return result;
        }

        [Route("Sync")]
        [HttpGet]
        public void Method()
        {
            Thread.Sleep(10000);
        }

        [Route("Async")]
        [HttpGet]
        public async Task MethodAsync()
        {
            await Task.Delay(10000);
        }
    }
}
