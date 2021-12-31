using Backend.Attributes.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "請輸入新密碼")]
        [StringSame("NewPasswordAgain", ErrorMessage = "兩次輸入的密碼必須相同")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "請輸入第二次新密碼")]
        public string NewPasswordAgain { get; set; }
    }
}
