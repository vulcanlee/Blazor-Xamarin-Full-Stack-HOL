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
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }

        public int PhaseCategoryId { get; set; }
        public virtual PhaseCategory LeaveCategory { get; set; }
    }
}
