using Business.DataModel;
using Business.Services;
using DataTransferObject.DTOs;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ServiceHelps
{
    public class LoginUpdateTokenHelper
    {
        public static async Task<bool> UserLoginAsync(IPageDialogService dialogService,
            LoginService loginService, SystemStatusService systemStatusService, LoginRequestDto loginRequestDTO,
            AppStatus appStatus)
        {
            var fooResult = await loginService.PostAsync(loginRequestDTO);
            if (fooResult.Status != true)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                return false;
            }

            systemStatusService.SingleItem.UserID = loginService.SingleItem.Id;
            systemStatusService.SingleItem.Account = loginService.SingleItem.Account;
            systemStatusService.SingleItem.IsLogin = true;
            systemStatusService.SingleItem.LoginedTime = DateTime.Now;
            systemStatusService.SingleItem.Token = loginService.SingleItem.Token;
            systemStatusService.SingleItem.RefreshToken = loginService.SingleItem.RefreshToken;
            systemStatusService.SingleItem.TokenExpireMinutes = loginService.SingleItem.TokenExpireMinutes;
            systemStatusService.SingleItem.RefreshTokenExpireDays = loginService.SingleItem.RefreshTokenExpireDays;
            systemStatusService.SingleItem.SetExpireDatetime();

            //await systemStatusService.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusService, appStatus);

            return true;
        }
        public static async Task<bool> UserLogoutAsync(IPageDialogService dialogService,
         LoginService loginService, SystemStatusService systemStatusService,
         AppStatus appStatus)
        {
            await systemStatusService.ReadFromFileAsync();
            await loginService.ReadFromFileAsync();
            loginService.SingleItem = new LoginResponseDto();
            await loginService.WriteToFileAsync();

            systemStatusService.SingleItem.UserID = loginService.SingleItem.Id;
            systemStatusService.SingleItem.Account = loginService.SingleItem.Account;
            systemStatusService.SingleItem.IsLogin = false;
            systemStatusService.SingleItem.LoginedTime = DateTime.Now;
            systemStatusService.SingleItem.Token = loginService.SingleItem.Token;
            systemStatusService.SingleItem.RefreshToken = loginService.SingleItem.RefreshToken;
            systemStatusService.SingleItem.TokenExpireMinutes = loginService.SingleItem.TokenExpireMinutes;
            systemStatusService.SingleItem.RefreshTokenExpireDays = loginService.SingleItem.RefreshTokenExpireDays;
            systemStatusService.SingleItem.SetExpireDatetime();

            //await systemStatusService.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusService, appStatus);

            return true;
        }
    }
}
