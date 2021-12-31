using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class OnlyAdministratorDto : ICloneable, INotifyPropertyChanged
    {
        public string Message { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public OnlyAdministratorDto Clone()
        {
            return ((ICloneable)this).Clone() as OnlyAdministratorDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
