using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class MyUserAdapterModel : ICloneable
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

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(Account))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
