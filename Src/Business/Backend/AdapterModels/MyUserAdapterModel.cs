using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class MyUserAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        public string Password { get; set; }
        public string PasswordPlaintext { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string Salt { get; set; }
        public bool Status { get; set; }
        public DateTime ForceLogoutDatetime { get; set; }
        public int MenuRoleId { get; set; }
        public string MenuRoleName { get; set; }
        public virtual MenuRoleAdapterModel MenuRole { get; set; }

        public MyUserAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MyUserAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
