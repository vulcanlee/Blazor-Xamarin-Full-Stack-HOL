using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DataModel
{
    public class AppStatus
    {
        public SystemStatus SystemStatus { get; set; } = new SystemStatus();
        public string NotificationToken { get; set; } = "";
    }
}
