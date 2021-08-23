using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class AuditHistory
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUser MyUser { get; set; }
        public bool Approve { get; set; }
        [Required(ErrorMessage = "批示意見 不可為空白")]
        public string Comment { get; set; }
        public DateTime Updatetime { get; set; }
        public int AuditMasterId { get; set; }
        public AuditMaster AuditMaster { get; set; }
    }
    
}
