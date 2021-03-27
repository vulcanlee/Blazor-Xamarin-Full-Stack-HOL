using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class TravelExpenseDetailDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "主題 不可為空白")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public decimal Expense { get; set; }

        public int TravelExpenseId { get; set; }
        public virtual TravelExpenseDto TravelExpense { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TravelExpenseDetailDto Clone()
        {
            return ((ICloneable)this).Clone() as TravelExpenseDetailDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        public string Validation()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(Title))
            {
                sb.Append($"需要輸入 主題  {Environment.NewLine}");
            }
            if (string.IsNullOrEmpty(Summary))
            {
                sb.Append($"需要輸入 費用摘要說明  {Environment.NewLine}");
            }
            if (Expense <= 0)
            {
                sb.Append($"費用 不可小於 0 {Environment.NewLine}");
            }
            return sb.ToString();
        }
    }
}
