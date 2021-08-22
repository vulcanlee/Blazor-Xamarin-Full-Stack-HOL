using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domains.Models
{
    public partial class AuditMaster
    {
        public ICollection<AuditUser> AuditUser { get; set; }
        public AuditMaster()
        {
            AuditUser = new HashSet<AuditUser>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "主旨 不可為空白")]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }
        public int PolicyHeaderId { get; set; }
        public virtual PolicyHeader PolicyHeader { get; set; }
    }
}
