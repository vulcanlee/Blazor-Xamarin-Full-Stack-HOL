using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class MyUserPasswordHistoryAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel? MyUser { get; set; }
        public string Password { get; set; } = String.Empty;
        public DateTime ChangePasswordDatetime { get; set; }
        public string? IP { get; set; }

        public MyUserPasswordHistoryAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MyUserPasswordHistoryAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
