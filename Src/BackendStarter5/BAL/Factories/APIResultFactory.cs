using DTOs.DataModels;
using Microsoft.AspNetCore.Http;
using BAL.Helpers;
using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BAL.Factories
{
    public static class APIResultFactory
    {
        public static APIResult Build(bool aPIResultStatus,
            int statusCodes = StatusCodes.Status200OK, ErrorMessageEnum errorMessageEnum = ErrorMessageEnum.None,
            object payload = null, string exceptionMessage = "", bool replaceExceptionMessage = true)
        {
            APIResult apiResult = new APIResult()
            {
                Status = aPIResultStatus,
                ErrorCode = (int)errorMessageEnum,
                Message = (errorMessageEnum == ErrorMessageEnum.None) ? "" : $"錯誤代碼 {(int)errorMessageEnum}, {ErrorMessageMappingHelper.Instance.GetErrorMessage(errorMessageEnum)}",
                HTTPStatus = statusCodes,
                Payload = payload,
            };
            if (apiResult.ErrorCode == (int)ErrorMessageEnum.Exception)
            {
                apiResult.Message = $"{apiResult.Message}{exceptionMessage}";
            }
            else if (string.IsNullOrEmpty(exceptionMessage) == false)
            {
                if (replaceExceptionMessage == true)
                {
                    apiResult.Message = $"{exceptionMessage}";
                }
                else
                {
                    apiResult.Message += $"{exceptionMessage}";
                }
            }
            return apiResult;
        }
        /// <summary>
        /// 直接指定字串作為錯誤訊息，不用透過錯誤訊息列舉
        /// </summary>
        public static APIResult Build(bool aPIResultStatus,
            int statusCodes = StatusCodes.Status200OK, string errorMessage = "",
            object payload = null, string exceptionMessage = "", bool replaceExceptionMessage = true)
        {
            APIResult apiResult = new APIResult()
            {
                Status = aPIResultStatus,
                ErrorCode = 0,
                Message = $"{errorMessage}",
                HTTPStatus = statusCodes,
                Payload = payload,
            };
            if (apiResult.ErrorCode == (int)ErrorMessageEnum.Exception)
            {
                apiResult.Message = $"{apiResult.Message}{exceptionMessage}";
            }
            else if (string.IsNullOrEmpty(exceptionMessage) == false)
            {
                if (replaceExceptionMessage == true)
                {
                    apiResult.Message = $"{exceptionMessage}";
                }
                else
                {
                    apiResult.Message += $"{exceptionMessage}";
                }
            }
            return apiResult;
        }
    }
}
