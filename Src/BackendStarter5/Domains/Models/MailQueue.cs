using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    [Index(nameof(Status))]
    public partial class MailQueue
    {
        public long Id { get; set; }
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? SendedAt { get; set; }
        public int Status { get; set; }
        public int SendTimes { get; set; }
    }
}
