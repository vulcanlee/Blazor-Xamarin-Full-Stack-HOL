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
        public bool IsProcess { get; set; }
        public bool Approve { get; set; }
        public string Summary { get; set; }
        public string Comment { get; set; }
        public DateTime ReceiveTime { get; set; }
        public DateTime SendTime { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMaster FlowMaster { get; set; }
    }
    
}
