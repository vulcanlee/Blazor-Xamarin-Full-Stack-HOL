using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class MyUser
    {
        public ICollection<MyUserPasswordHistory> MyUserPasswordHistory { get; set; }
        public MyUser()
        {
            MyUserPasswordHistory = new HashSet<MyUserPasswordHistory>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; } = String.Empty;
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; } = String.Empty;
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; } = String.Empty;
        public string? Salt { get; set; }
        public bool Status { get; set; }
        public string? Email { get; set; }
        public int LoginFailTimes { get; set; }
        public DateTime LoginFailUnlockDatetime { get; set; }
        public DateTime ForceLogoutDatetime { get; set; }
        public bool ForceChangePassword { get; set; }
        public DateTime ForceChangePasswordDatetime { get; set; }
        public DateTime LastLoginDatetime { get; set; }
        public int MenuRoleId { get; set; }
        public MenuRole? MenuRole { get; set; }

    }
}
