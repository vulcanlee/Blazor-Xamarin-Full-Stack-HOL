using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class LeaveCategoryAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "請假類別 不可為空白")]
        public string Name { get; set; }

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public LeaveCategoryAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as LeaveCategoryAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
