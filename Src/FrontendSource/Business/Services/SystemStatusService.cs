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
            SetDefaultPersistentBehavior();
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = false;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }
    }
}
