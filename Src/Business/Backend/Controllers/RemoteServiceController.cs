using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {
        [HttpGet("AddAsync/{value1}/{value2}/{delay}")]
        public async Task<string> AddAsync(int value1, int value2, int delay)
        {
            DateTime Begin = DateTime.Now;

            #region 取得當前執行緒集區的狀態
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            #endregion

            await Task.Delay(delay);

            int sum = value1 + value2;

            DateTime Complete = DateTime.Now;
            return $"Result:{sum} " +
                $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
             $" ({Begin:ss} - {Complete:ss} = {(Complete - Begin).TotalSeconds})";
        }
        [HttpGet("Add/{value1}/{value2}/{delay}")]
        public string Add(int value1, int value2, int delay)
        {
            DateTime Begin = DateTime.Now;
            #region 取得當前執行緒集區的狀態
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            #endregion

            Thread.Sleep(delay);

            int sum = value1 + value2;

            DateTime Complete = DateTime.Now;
            return $"Result:{sum} " +
             $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
             $" ({Begin:ss} - {Complete:ss} = {(Complete - Begin).TotalSeconds})";
        }
        [HttpGet("SetThreadPool/{value1}/{value2}")]
        public string SetThreadPool(int value1, int value2)
        {
            ThreadPool.SetMinThreads(value1, value2);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }
        [HttpGet("SetMaxThreadPool/{value1}/{value2}")]
        public string SetMaxThreadPool(int value1, int value2)
        {
            ThreadPool.SetMaxThreads(value1, value2);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }
        [HttpGet("GetThreadPool")]
        public string GetThreadPool()
        {
            string result = "";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            //var memoryMetrics = GetWindowsMetrics();
            result = "" +
                $" Processor:{Environment.ProcessorCount} " +
                $" AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }
        private MemoryMetrics GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }
    }
    public class MemoryMetrics
    {
        public double Total;
        public double Used;
        public double Free;
    }
}
