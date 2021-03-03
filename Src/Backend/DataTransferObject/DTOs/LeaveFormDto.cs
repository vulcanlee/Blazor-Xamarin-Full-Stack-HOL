using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class LeaveFormDto
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
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int LeaveCategoryId { get; set; }
        public string LeaveCategoryName { get; set; }
        public virtual LeaveCategoryDto LeaveCategory { get; set; }
    }
}
