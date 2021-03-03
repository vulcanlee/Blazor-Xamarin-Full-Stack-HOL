using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class TravelExpenseAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public decimal TotalExpense { get; set; }

        public int MyUserId { get; set; }
        public string MyUserName{ get; set; }
        public virtual MyUserAdapterModel MyUser { get; set; }

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(ApplyDate.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public TravelExpenseAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as TravelExpenseAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
