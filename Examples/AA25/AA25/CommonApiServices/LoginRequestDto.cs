using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA25.CommonApiServices
{
    public class LoginRequestDto : ICloneable, INotifyPropertyChanged
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginRequestDto Clone()
        {
            return ((ICloneable)this).Clone() as LoginRequestDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
