using System;

namespace AC03.Services
{
    public class MyService : IMyService, IMyServiceTransient, IMyServiceScoped, IMyServiceSingleton
    {
        public int HashCode
        {
            get
            {
                return this.GetHashCode();
            }
        }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
