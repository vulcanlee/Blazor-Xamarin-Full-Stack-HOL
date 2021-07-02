using DTOs.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using BAL.Factories;
using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Helpers
{
    public class JWTTokenFailHelper
    {
        public static APIResult GetFailResult(Exception  exception)
        {
            APIResult apiResult;
            // SecurityTokenValidationException 當接收到的安全性權杖無效時所擲回的例外狀況。
            // https://docs.microsoft.com/zh-tw/dotnet/api/system.identitymodel.tokens.securitytokenvalidationexception?view=netframework-4.8

            var nameis = exception.GetType().Name;
            if (exception is SecurityTokenNotYetValidException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    ErrorMessageEnum.SecurityTokenNotYetValidException,null, $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenExpiredException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    ErrorMessageEnum.SecurityTokenExpiredException, null, $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenReplayDetectedException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    ErrorMessageEnum.SecurityTokenReplayDetectedException, null, $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenValidationException) 
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    ErrorMessageEnum.SecurityTokenValidationException, null, $" {nameis} {exception.Message}", false);
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    ErrorMessageEnum.AuthenticationFailed, null, $" {nameis} {exception.Message}", false);
            }
            return apiResult;

        }
    }
}
