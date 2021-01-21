using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class SystemEnvironmentResponseDTO
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AndroidVersion { get; set; }
        public string AndroidUrl { get; set; }
        public string iOSVersion { get; set; }
        public string iOSUrl { get; set; }
    }
}
