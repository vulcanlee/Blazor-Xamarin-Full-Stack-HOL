using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class MyUserDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public bool IsManager { get; set; }
        public string ManagerName { get; set; }
        public string IsManagerString { get; set; }
        public string Salt { get; set; }
        public bool Status { get; set; }
        public int MenuRoleId { get; set; }
        public virtual MenuRoleDto MenuRole { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public MyUserDto Clone()
        {
            return ((ICloneable)this).Clone() as MyUserDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
