using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class MyUserAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string PasswordPlaintext { get; set; } = String.Empty;
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; } = String.Empty;
        public string? Salt { get; set; }
        public bool Status { get; set; }
        [EmailAddress(ErrorMessage ="請輸入正確的電子郵件信箱")]
        public string? Email { get; set; }
        public int LoginFailTimes { get; set; }
        public DateTime LoginFailUnlockDatetime { get; set; }
        public DateTime ForceLogoutDatetime { get; set; }
        public bool ForceChangePassword { get; set; }
        public DateTime ForceChangePasswordDatetime { get; set; }
        public DateTime LastLoginDatetime { get; set; }
        public int MenuRoleId { get; set; }
        public string MenuRoleName { get; set; } = String.Empty;
        public MenuRoleAdapterModel? MenuRole { get; set; }

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
