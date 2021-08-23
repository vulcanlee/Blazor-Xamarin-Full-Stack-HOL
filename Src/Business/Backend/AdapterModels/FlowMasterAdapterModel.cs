﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class FlowMasterAdapterModel : ICloneable
    {
        public ICollection<FlowUserAdapterModel> FlowUser { get; set; }
        public FlowMasterAdapterModel()
        {
            FlowUser = new HashSet<FlowUserAdapterModel>();
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

        public FlowMasterAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as FlowMasterAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}