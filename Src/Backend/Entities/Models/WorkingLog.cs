using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    /// <summary>
    /// 每日工作日誌
    /// </summary>
    public class WorkingLog
    {
        public WorkingLog()
        {
            WorkingLogDetail = new HashSet<WorkingLogDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TotalHours { get; set; }

        public int MyUserId { get; set; }
        public virtual MyUser MyUser { get; set; }

        public virtual ICollection<WorkingLogDetail> WorkingLogDetail { get; set; }
    }
}
