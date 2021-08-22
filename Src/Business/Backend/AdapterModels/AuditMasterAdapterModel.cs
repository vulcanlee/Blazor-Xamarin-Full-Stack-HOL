using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class AuditMasterAdapterModel : ICloneable
    {
        public ICollection<AuditUserAdapterModel> AuditUser { get; set; }
        public AuditMasterAdapterModel()
        {
            AuditUser = new HashSet<AuditUserAdapterModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "主旨 不可為空白")]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public int PolicyHeaderId { get; set; }
        public virtual PolicyHeaderAdapterModel PolicyHeader { get; set; }
        public string PolicyHeaderName { get; set; }

        public AuditMasterAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as AuditMasterAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
