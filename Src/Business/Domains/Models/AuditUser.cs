using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class AuditUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MyUserId { get; set; }
        public MyUser MyUser { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        public bool Enable { get; set; }
        public int AuditMasterId { get; set; }
        public AuditMaster AuditMaster { get; set; }
    }
}
