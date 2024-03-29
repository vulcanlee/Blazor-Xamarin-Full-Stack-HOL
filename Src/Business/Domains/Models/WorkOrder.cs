﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domains.Models
{
    [Index(nameof(Code))]
    public partial class WorkOrder
    {
        //public virtual ICollection<OrderItem> OrderItems { get; set; }
        public WorkOrder()
        {
            //OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        [StringLength(13)]
        public string Code { get; set; }
        /// <summary>
        /// 問題描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }
        public int CategoryMainId { get; set; }
        public CategoryMain CategoryMain { get; set; }
        public int CategorySubId { get; set; }
        public CategorySub CategorySub { get; set; }
        public int EngineerId { get; set; }
    }
}
