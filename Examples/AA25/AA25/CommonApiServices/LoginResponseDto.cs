using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA25.CommonApiServices
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

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginResponseDto Clone()
        {
            return ((ICloneable)this).Clone() as LoginResponseDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
