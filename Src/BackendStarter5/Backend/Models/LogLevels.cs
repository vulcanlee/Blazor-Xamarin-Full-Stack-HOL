using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class LogLevels
    {
        public static string Trace { get; set; } = "Trace";
        public static string Debug { get; set; } = "Debug";
        public static string Information { get; set; } = "Information";
        public static string Warning { get; set; } = "Warning";
        public static string Error { get; set; } = "Error";
        public static string Critical { get; set; } = "Critical";
    }
}
