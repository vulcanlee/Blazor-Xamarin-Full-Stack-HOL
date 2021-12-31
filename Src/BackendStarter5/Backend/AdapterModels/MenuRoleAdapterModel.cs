using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.AdapterModels
{
    public class MenuRoleAdapterModel : ICloneable
    {
        public MenuRoleAdapterModel()
        {
            MenuData = new HashSet<MenuDataAdapterModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; } = String.Empty;
        public string? Remark { get; set; }

        public virtual ICollection<MenuDataAdapterModel> MenuData { get; set; }

        public MenuRoleAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MenuRoleAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
