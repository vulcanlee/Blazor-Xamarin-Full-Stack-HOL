using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 專案名稱
    /// </summary>
    public class Project
    {
        public Project()
        {
            WorkingLogDetail = new HashSet<WorkingLogDetail>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "專案名稱 不可為空白")]
        public string Name { get; set; }
  
        public virtual ICollection<WorkingLogDetail> WorkingLogDetail { get; set; }
    }
}
