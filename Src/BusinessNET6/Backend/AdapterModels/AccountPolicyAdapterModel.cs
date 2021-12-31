using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class AccountPolicyAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public bool EnableLoginFailDetection { get; set; }
        public int LoginFailMaxTimes { get; set; }
        public int LoginFailTimesLockMinutes { get; set; }
        public bool EnableCheckPasswordAge { get; set; }
        public int PasswordAge { get; set; }
        public int MinimumPasswordLength { get; set; }
        public int PasswordHistory { get; set; }
        public bool EnablePasswordHistory { get; set; }
        public int PasswordComplexity { get; set; }

        public AccountPolicyAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as AccountPolicyAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
