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
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SendedAt { get; set; }
        public int Status { get; set; }
        public int SendTimes { get; set; }
    }
}
