using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class AppExceptionsService : BaseWebAPI<ExceptionRecordDto>
    {
        public AppExceptionsService()
            : base()
        {
            SetDefaultPersistentBehavior();
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = true;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }
    }
}
