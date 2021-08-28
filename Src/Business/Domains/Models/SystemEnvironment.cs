using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Models
{
    public class SystemEnvironment
    {
        public int Id { get; set; }
        public int LoginFailMaxTimes { get; set; }
        public int LoginFailTimesLockMinutes { get; set; }
    }
}
