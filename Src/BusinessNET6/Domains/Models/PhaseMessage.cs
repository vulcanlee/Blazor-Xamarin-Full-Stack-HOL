using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    /// <summary>
    /// 輸入片語文字
    /// </summary>
    public class PhaseMessage
    {
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        [Required(ErrorMessage = "內容 不可為空白")]
        public string Content { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }

        public int PhaseCategoryId { get; set; }
        public virtual PhaseCategory PhaseCategory { get; set; }
    }
}
