using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class MyUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string Salt { get; set; }
        public bool Status { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public bool IsManager { get; set; }
        public int MenuRoleId { get; set; }
        public virtual MenuRole MenuRole { get; set; }
    }
}
