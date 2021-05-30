using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "請輸入舊密碼")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "請輸入新密碼")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "請輸入第二次新密碼")]
        public string NewPasswordAgain { get; set; }
    }
}
