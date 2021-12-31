using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class LoginResponseDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public int TokenExpireMinutes { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireDays { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginResponseDto Clone()
        {
            return ((ICloneable)this).Clone() as LoginResponseDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
