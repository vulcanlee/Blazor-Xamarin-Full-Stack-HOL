using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 請假類別
    /// </summary>
    public class LeaveCategory
    {
        public LeaveCategory()
        {
            LeaveForm = new HashSet<LeaveForm>();
        }

        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "請假類別 不可為空白")]
        public string Name { get; set; }

        public virtual ICollection<LeaveForm> LeaveForm { get; set; }
    }
}
