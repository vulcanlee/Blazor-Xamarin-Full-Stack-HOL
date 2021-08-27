using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class FlowInbox
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUser MyUser { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public string Body { get; set; }
        public DateTime ReceiveTime { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMaster FlowMaster { get; set; }
    }
    
}
