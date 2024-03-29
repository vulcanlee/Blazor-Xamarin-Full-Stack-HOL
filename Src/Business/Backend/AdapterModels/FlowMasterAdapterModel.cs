﻿using Backend.Models;
using System;
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
        [StringLength(13)]
        public string Code { get; set; }
        [Required(ErrorMessage = "主旨 不可為空白")]
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 正在進行簽核流程階層
        /// </summary>
        public int ProcessLevel { get; set; }
        /// <summary>
        /// 此簽核流程狀態碼
        /// </summary>
        public int Status { get; set; }
        public string StatusName { get; set; }
        /// <summary>
        /// 簽核流程的嵌入來源 Json 紀錄
        /// </summary>
        public string SourceJson { get; set; }
        /// <summary>
        /// 簽核來源的類型
        /// </summary>
        public FlowSourceTypeEnum SourceType { get; set; }
        public string SourceCode { get; set; } = "";
        public string SourceTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateAt { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public string NextMyUserName { get; set; }
        public int PolicyHeaderId { get; set; }
        public virtual PolicyHeaderAdapterModel PolicyHeader { get; set; }
        public string PolicyHeaderName { get; set; }
        public bool UserShowAction { get; set; }

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
