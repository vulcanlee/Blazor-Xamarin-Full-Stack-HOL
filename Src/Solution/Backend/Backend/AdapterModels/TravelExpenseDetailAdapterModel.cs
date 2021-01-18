using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class TravelExpenseDetailAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "主題 不可為空白")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public decimal Expense { get; set; }

        public int TravelExpenseId { get; set; }
        public virtual TravelExpenseAdapterModel TravelExpense { get; set; }

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

        public TravelExpenseDetailAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as TravelExpenseDetailAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
