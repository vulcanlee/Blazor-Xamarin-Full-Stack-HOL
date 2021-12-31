using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.AdapterModels
{
    public class MenuDataAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; } = String.Empty;
        [Range(0, 1, ErrorMessage = "僅能輸入 0 或者 1 不可為空白")]
        public int Level { get; set; }
        public bool IsGroup { get; set; }
        public string IsGroupName { get; set; } = String.Empty;
        public int Sequence { get; set; }
        public string Icon { get; set; } = String.Empty;
        [Required(ErrorMessage = "作業名稱 不可為空白")]
        public string CodeName { get; set; } = String.Empty;
        public bool Enable { get; set; }
        public string EnableName { get; set; } = String.Empty;
        [Required(ErrorMessage = "驗證碼 不可為空白")]
        public bool ForceLoad { get; set; }
        public bool NewTab { get; set; } = false;
        public int MenuRoleId { get; set; }
        public virtual MenuRoleAdapterModel? MenuRole { get; set; }

        public MenuDataAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MenuDataAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
