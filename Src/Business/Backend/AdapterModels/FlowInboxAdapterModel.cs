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
        public bool IsProcess { get; set; }
        public string IsProcessName
        {
            get
            {
                return IsProcess ? "已處理" : "未處理";
            }
        }
        public bool Approve { get; set; }
        public string Summary { get; set; }
        public string Comment { get; set; }
        public DateTime ReceiveTime { get; set; }
        public DateTime SendTime { get; set; }
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
