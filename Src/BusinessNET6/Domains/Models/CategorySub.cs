using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domains.Models
{
    public class CategorySub
    {
        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
        public CategorySub()
        {
            WorkOrder = new HashSet<WorkOrder>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public bool Enable { get; set; }
        public int CategoryMainId { get; set; }
        public CategoryMain CategoryMain { get; set; }
    }
}
