using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public partial class MenuRole
    {
        public MenuRole()
        {
            MenuData = new HashSet<MenuData>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<MenuData> MenuData { get; set; }
    }
}
