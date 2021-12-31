using DTOs.DataModels;
using Microsoft.AspNetCore.Http;
using CommonDomain.DataModels;
using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Factories
{
    public static class VerifyRecordResultFactory
    {
        public static VerifyRecordResult Build(bool success, ErrorMessageEnum errorMessageEnum = ErrorMessageEnum.None)
        {
            VerifyRecordResult verifyRecordResult = new VerifyRecordResult()
            {
                Success = success,
                MessageId = errorMessageEnum,
                Message = "",
                Exception = null,
            };
            return verifyRecordResult;
        }

        /// <summary>
        /// 使用文字內容來回應
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static VerifyRecordResult Build(bool success, string message, Exception exception=null)
        {
            VerifyRecordResult verifyRecordResult = new VerifyRecordResult()
            {
                Success = success,
                MessageId = ErrorMessageEnum.客製化文字錯誤訊息,
                Message = message,
                Exception = exception,
            };
            return verifyRecordResult;
        }

    }
}
