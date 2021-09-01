using Backend.AdapterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ApproveOpinionModel
    {
        public PolicyHeaderAdapterModel PolicyHeaderAdapterModel { get; set; }
        [Required(ErrorMessage = "批示摘要 不可為空白")]
        public string Summary { get; set; } = "";
        [Required(ErrorMessage = "批示意見 不可為空白")]
        public string Comment { get; set; } = "";
    }
}
