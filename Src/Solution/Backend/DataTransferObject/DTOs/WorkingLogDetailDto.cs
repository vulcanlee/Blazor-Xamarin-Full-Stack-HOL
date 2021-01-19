using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public partial class WorkingLogDetailDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "工作項目 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "工作項目說明 不可為空白")]
        public string Summary { get; set; }
        public decimal Hours { get; set; }

        public int WorkingLogId { get; set; }
        public virtual WorkingLogDto WorkingLog { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public virtual ProjectDto Project { get; set; }
    }
}
