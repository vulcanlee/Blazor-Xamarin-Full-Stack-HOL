using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DataModels
{
    public partial class MenuDataDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        [Range(0, 1, ErrorMessage = "僅能輸入 0 或者 1 不可為空白")]
        public int Level { get; set; }
        public bool IsGroup { get; set; }
        public int Sequence { get; set; }
        public string Icon { get; set; }
        [Required(ErrorMessage = "作業名稱 不可為空白")]
        public string CodeName { get; set; }
        public bool Enable { get; set; }
        [Required(ErrorMessage = "驗證碼 不可為空白")]
        public string VerifyCode { get; set; }
        public Guid Guid { get; set; }
        public bool ForceLoad { get; set; }
        public int MenuRoleId { get; set; }
        public virtual MenuRoleDto MenuRole { get; set; }

    }
}
