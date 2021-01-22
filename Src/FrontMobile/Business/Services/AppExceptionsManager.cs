using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class AppExceptionsManager : BaseWebAPI<ExceptionRecordDto>
    {
        public AppExceptionsManager()
            : base()
        {
        }
    }
}
