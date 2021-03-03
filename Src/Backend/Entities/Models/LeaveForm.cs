using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 請假單
    /// </summary>
    public class LeaveForm
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public decimal Hours { get; set; }
        [Required(ErrorMessage = "請假事由 不可為空白")]
        public string LeaveCause { get; set; }
        public string FormsStatus { get; set; }
        public string ApproveResult { get; set; }

        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }
        public int AgentId { get; set; }
        public int LeaveCategoryId { get; set; }
        public virtual LeaveCategory LeaveCategory { get; set; }
    }
}
