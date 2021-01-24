using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class OnlyAdministratorDto : ICloneable, INotifyPropertyChanged
    {
        public string Message { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LeaveFormDto Clone()
        {
            return ((ICloneable)this).Clone() as LeaveFormDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
