using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 公司緊急連絡方式
    /// </summary>
    public class OnCallPhone
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "電話號碼 不可為空白")]
        public string PhoneNumber { get; set; }
    }
}
