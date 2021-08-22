using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class AuditMaster
    {
        public AuditMaster()
        {
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }
        public int PolicyHeaderId { get; set; }
        public virtual PolicyHeader PolicyHeader { get; set; }
        public string PolicyHeaderName { get; set; }
    }
}
