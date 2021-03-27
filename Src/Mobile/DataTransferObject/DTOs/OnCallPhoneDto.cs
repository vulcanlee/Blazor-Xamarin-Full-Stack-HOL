using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class OnCallPhoneDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "電話號碼 不可為空白")]
        public string PhoneNumber { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OnCallPhoneDto Clone()
        {
            return ((ICloneable)this).Clone() as OnCallPhoneDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
