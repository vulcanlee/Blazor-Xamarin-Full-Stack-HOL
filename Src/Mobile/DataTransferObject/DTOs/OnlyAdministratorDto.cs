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

        public OnlyAdministratorDto Clone()
        {
            return ((ICloneable)this).Clone() as OnlyAdministratorDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
