using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 每日工作日誌明細
    /// </summary>
    public class WorkingLogDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "主題 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "主題說明 不可為空白")]
        public string Summary { get; set; }
        public decimal Hours { get; set; }

        public int WorkingLogId { get; set; }
        public virtual WorkingLog WorkingLog { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
