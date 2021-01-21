using Business.DataModel;
using CommonLibrary.Helpers.WebAPIs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class SystemStatusManager : BaseWebAPI<SystemStatus>
    {
        public SystemStatusManager()
            : base()
        {
            isCollection = false;
        }
    }
}
