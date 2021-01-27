using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class TravelExpenseDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public decimal TotalExpense { get; set; }

        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TravelExpenseDto Clone()
        {
            return ((ICloneable)this).Clone() as TravelExpenseDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        public string Validation()
        {
            StringBuilder sb = new StringBuilder();
            if (MyUserId <= 0)
            {
                sb.Append($"需要選擇一個 申請人 {Environment.NewLine}");
            }
            return sb.ToString();
        }
    }
}
