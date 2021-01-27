using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class OnlyUserDto : ICloneable, INotifyPropertyChanged
    {
        public string Message { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OnlyUserDto Clone()
        {
            return ((ICloneable)this).Clone() as OnlyUserDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        public string Validation()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
