using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class FlowHistoryAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public bool Approve { get; set; }
        public string ApproveName
        {
            get
            {
                return Approve ? "通過" : "拒絕";
            }
        }
        public string Summary { get; set; }
        [Required(ErrorMessage = "批示意見 不可為空白")]
        public string Comment { get; set; }
        public DateTime Updatetime { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMasterAdapterModel FlowMaster { get; set; }
        public string FlowMasterName { get; set; }

        public FlowHistoryAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as FlowHistoryAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
