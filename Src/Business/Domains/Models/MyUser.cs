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
        public MyUser()
        {
            PolicyDetail = new HashSet<PolicyDetail>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string Salt { get; set; }
        public bool Status { get; set; }
        public DateTime ForceLogoutDatetime { get; set; }
        public bool ForceChangePassword { get; set; }
        public DateTime ForceChangePasswordDatetime { get; set; }
        public int MenuRoleId { get; set; }
        public virtual MenuRole MenuRole { get; set; }

        public virtual ICollection<PolicyDetail> PolicyDetail { get; set; }
    }
}
