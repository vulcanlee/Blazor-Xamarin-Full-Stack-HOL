using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class FlowInboxAdapterModel : ICloneable
    {
        public FlowInboxAdapterModel()
        {
        }
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public string IsReadName
        {
            get
            {
                return IsRead ? "已讀" : "未讀";
            }
        }
        public string Body { get; set; }
        public DateTime ReceiveTime { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMasterAdapterModel FlowMaster { get; set; }
        public string FlowMasterName { get; set; }

        public FlowInboxAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as FlowInboxAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
