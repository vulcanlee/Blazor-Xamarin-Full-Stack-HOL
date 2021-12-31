using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Models
{
    public class ExceptionRecord
    {
        public int Id { get; set; }
        public int? MyUserId { get; set; }

        public string? DeviceName { get; set; }
        public string? DeviceModel { get; set; }
        public string? OSType { get; set; }
        public string? OSVersion { get; set; }
        public string? Message { get; set; }
        public string? CallStack { get; set; }
        public DateTime ExceptionTime { get; set; }
  
        public MyUser? MyUser { get; set; }
    }
}
