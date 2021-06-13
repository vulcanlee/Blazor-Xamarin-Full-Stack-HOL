using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
