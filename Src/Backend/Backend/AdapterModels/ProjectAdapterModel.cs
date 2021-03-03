using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class ProjectAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "專案名稱 不可為空白")]
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

        public ProjectAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as ProjectAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
