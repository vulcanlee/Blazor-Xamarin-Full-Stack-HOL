using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class WorkingLogAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TotalHours { get; set; }
        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserAdapterModel MyUser { get; set; }

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

        public WorkingLogAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as WorkingLogAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
