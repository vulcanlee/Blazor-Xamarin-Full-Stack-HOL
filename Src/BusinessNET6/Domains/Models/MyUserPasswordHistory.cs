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
    public class MyUserPasswordHistory
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUser? MyUser { get; set; }
        public string Password { get; set; } = String.Empty;
        public DateTime ChangePasswordDatetime { get; set; }
        public string? IP { get; set; }
    }
}
