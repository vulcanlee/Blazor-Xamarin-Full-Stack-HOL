using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ConfigurationModel
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Tokens Tokens { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
        public Customnlog CustomNLog { get; set; }
        public string KeepAliveEndpoint { get; set; }
        public bool EnableKeepAliveEndpoint { get; set; }
        public bool ShowImportanceConfigurationInforation { get; set; }
        public bool EmergenceDebug { get; set; }
        public string AdministratorPassword { get; set; }
        public int CheckUserStateInterval { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Tokens
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int JwtRefreshExpireDays { get; set; }
        public string IssuerSigningKey { get; set; }
    }

    public class Connectionstrings
    {
        public string BackendDefaultConnection { get; set; }
    }

    public class Customnlog
    {
        public string LogRootPath { get; set; }
        public string AllLogMessagesFilename { get; set; }
        public string AllWebDetailsLogMessagesFilename { get; set; }
    }
}
