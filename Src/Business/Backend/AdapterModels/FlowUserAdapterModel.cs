using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class FlowUserAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        /// <summary>
        /// 簽核流程已經完成
        /// </summary>
        public bool Completion { get; set; }
        public string CompletionName { get; set; }
        public bool Enable { get; set; }
        public int FlowMasterId { get; set; }
        public FlowMasterAdapterModel FlowMaster { get; set; }

        public FlowUserAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as FlowUserAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
