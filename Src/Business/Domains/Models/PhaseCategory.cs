using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    /// <summary>
    /// 輸入片語類別
    /// </summary>
    public class PhaseCategory
    {
        public PhaseCategory()
        {
            PhaseMessage = new HashSet<PhaseMessage>();
        }

        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }

        public virtual ICollection<PhaseMessage> PhaseMessage { get; set; }
    }
}
