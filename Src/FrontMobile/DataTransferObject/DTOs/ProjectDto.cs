using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "專案名稱 不可為空白")]
        public string Name { get; set; }
    }
}
