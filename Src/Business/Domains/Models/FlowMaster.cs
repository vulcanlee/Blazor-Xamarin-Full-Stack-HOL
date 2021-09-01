using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domains.Models
{
    [Index(nameof(Code))]
    public partial class FlowMaster
    {
        public virtual ICollection<FlowUser> FlowUser { get; set; }
        public virtual ICollection<FlowHistory> FlowHistory { get; set; }
        public FlowMaster()
        {
            FlowUser = new HashSet<FlowUser>();
            FlowHistory = new HashSet<FlowHistory>();
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
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 簽核流程的嵌入來源 Json 紀錄
        /// </summary>
        public string SourceJson { get; set; }
        /// <summary>
        /// 簽核來源的類型
        /// </summary>
        public int SourceType { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }
        public string NextMyUserName { get; set; }
        public int PolicyHeaderId { get; set; }
        public virtual PolicyHeader PolicyHeader { get; set; }
    }
}
