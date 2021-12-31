using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class OnlyUserDto : ICloneable, INotifyPropertyChanged
    {
        public string Message { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public OnlyUserDto Clone()
        {
            return ((ICloneable)this).Clone() as OnlyUserDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
