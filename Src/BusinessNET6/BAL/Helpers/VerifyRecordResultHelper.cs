using CommonDomain.DataModels;
using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Helpers
{
    public class VerifyRecordResultHelper
    {
        public static string GetMessageString(VerifyRecordResult verifyRecordResult)
        {
            if (verifyRecordResult.MessageId == CommonDomain.Enums.ErrorMessageEnum.客製化文字錯誤訊息)
            {
                return verifyRecordResult.Message;
            }
            else
            {
                return ErrorMessageMappingHelper.Instance.GetErrorMessage(verifyRecordResult.MessageId);
            }
        }
    }
}
