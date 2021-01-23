using Business.DataModel;
using CommonLibrary.Helpers.WebAPIs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class SystemStatusService : BaseWebAPI<SystemStatus>
    {
        public SystemStatusService()
            : base()
        {
            isCollection = false;
        }
    }
}
