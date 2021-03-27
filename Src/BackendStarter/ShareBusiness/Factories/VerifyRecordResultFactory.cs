using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Http;
using ShareDomain.DataModels;
using ShareDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBusiness.Factories
{
    public static class VerifyRecordResultFactory
    {
        public static VerifyRecordResult Build(bool success, ErrorMessageEnum errorMessageEnum = ErrorMessageEnum.None)
        {
            VerifyRecordResult verifyRecordResult = new VerifyRecordResult()
            {
                Success = success,
                MessageId = errorMessageEnum,
            };
            return verifyRecordResult;
        }
    }
}
