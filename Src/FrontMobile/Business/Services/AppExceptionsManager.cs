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
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            //this.url = "/api/ExceptionRecords";
            //this.host = "https://lobworkshop.azurewebsites.net";
        }
    }
}
