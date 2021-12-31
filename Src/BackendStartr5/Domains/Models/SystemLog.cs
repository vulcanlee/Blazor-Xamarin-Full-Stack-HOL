using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class SystemLog
    {
        public long Id { get; set; }
        public string? Category { get; set; }
        public string? LogLevel { get; set; }
        public string? Message { get; set; }
        public string? Content { get; set; }
        public string? IP { get; set; }
        public DateTime Updatetime { get; set; }
    }
}
