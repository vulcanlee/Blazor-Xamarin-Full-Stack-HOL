using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class LoginRequestDto : ICloneable, INotifyPropertyChanged
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginRequestDto Clone()
        {
            return ((ICloneable)this).Clone() as LoginRequestDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
