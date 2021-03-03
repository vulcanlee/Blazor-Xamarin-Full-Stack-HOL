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
        public MyUser()
        {
            WorkingLog = new HashSet<WorkingLog>();
            LeaveForm = new HashSet<LeaveForm>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public bool IsManager { get; set; }
   
        public virtual ICollection<WorkingLog> WorkingLog { get; set; }
        public virtual ICollection<LeaveForm> LeaveForm { get; set; }
    }
}
