using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class OnCallPhoneAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "電話號碼 不可為空白")]
        public string PhoneNumber { get; set; }

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public OnCallPhoneAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as OnCallPhoneAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
