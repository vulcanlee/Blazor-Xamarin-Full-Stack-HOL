using Business.DataModel;
using Business.Services;
using DataTransferObject.DTOs;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ManagerHelps
{
    public class LoginUpdateTokenHelper
    {
        public static async Task<bool> UserLoginAsync(IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager, LoginRequestDto loginRequestDTO,
            AppStatus appStatus)
        {
            var fooResult = await loginManager.PostAsync(loginRequestDTO);
            if (fooResult.Status != true)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                return false;
            }

            systemStatusManager.SingleItem.UserID = loginManager.SingleItem.Id;
            systemStatusManager.SingleItem.Account = loginManager.SingleItem.Account;
            systemStatusManager.SingleItem.IsLogin = true;
            systemStatusManager.SingleItem.LoginedTime = DateTime.Now;
            systemStatusManager.SingleItem.Token = loginManager.SingleItem.Token;
            systemStatusManager.SingleItem.RefreshToken = loginManager.SingleItem.RefreshToken;
            systemStatusManager.SingleItem.TokenExpireMinutes = loginManager.SingleItem.TokenExpireMinutes;
            systemStatusManager.SingleItem.RefreshTokenExpireDays = loginManager.SingleItem.RefreshTokenExpireDays;
            systemStatusManager.SingleItem.SetExpireDatetime();

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
        public static async Task<bool> UserLogoutAsync(IPageDialogService dialogService,
         LoginManager loginManager, SystemStatusManager systemStatusManager,
         AppStatus appStatus)
        {
            await systemStatusManager.ReadFromFileAsync();
            await loginManager.ReadFromFileAsync();
            loginManager.SingleItem = new LoginResponseDto();
            await loginManager.WriteToFileAsync();

            systemStatusManager.SingleItem.UserID = loginManager.SingleItem.Id;
            systemStatusManager.SingleItem.Account = loginManager.SingleItem.Account;
            systemStatusManager.SingleItem.IsLogin = false;
            systemStatusManager.SingleItem.LoginedTime = DateTime.Now;
            systemStatusManager.SingleItem.Token = loginManager.SingleItem.Token;
            systemStatusManager.SingleItem.RefreshToken = loginManager.SingleItem.RefreshToken;
            systemStatusManager.SingleItem.TokenExpireMinutes = loginManager.SingleItem.TokenExpireMinutes;
            systemStatusManager.SingleItem.RefreshTokenExpireDays = loginManager.SingleItem.RefreshTokenExpireDays;
            systemStatusManager.SingleItem.SetExpireDatetime();

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
    }
}
