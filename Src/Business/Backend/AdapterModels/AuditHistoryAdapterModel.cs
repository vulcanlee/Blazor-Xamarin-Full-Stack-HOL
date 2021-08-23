using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class AuditHistoryAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public bool Approve { get; set; }
        [Required(ErrorMessage = "批示意見 不可為空白")]
        public string Comment { get; set; }
        public DateTime Updatetime { get; set; }
        public int AuditMasterId { get; set; }
        public AuditMasterAdapterModel AuditMaster { get; set; }
        public string AuditMasterName { get; set; }

        public AuditHistoryAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as AuditHistoryAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
