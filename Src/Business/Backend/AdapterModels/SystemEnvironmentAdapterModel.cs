using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class SystemEnvironmentAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public bool EnableLoginFailDetection { get; set; }
        public int LoginFailMaxTimes { get; set; }
        public int LoginFailTimesLockMinutes { get; set; }
        public bool EnableCheckPasswordAge { get; set; }
        public int PasswordAge { get; set; }
        public int MinimumPasswordLength { get; set; }
        public int PasswordHistory { get; set; }
        public int PasswordComplexity { get; set; }

        public SystemEnvironmentAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as SystemEnvironmentAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
