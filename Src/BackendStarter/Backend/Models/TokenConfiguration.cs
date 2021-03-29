using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class TokenConfiguration
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int JwtRefreshExpireDays { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}
