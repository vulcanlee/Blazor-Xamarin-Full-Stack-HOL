using Business.DataModel;
using Business.Services;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ServiceHelps
{
    public class RefreshTokenHelper
    {
        public static async Task<bool> CheckAndRefreshToken(IPageDialogService dialogService,
            RefreshTokenService refreshTokenService, SystemStatusService systemStatusService,
            AppStatus appStatus)
        {
            if (appStatus.SystemStatus.TokenExpireDatetime > DateTime.Now)
            {
                #region Token 尚在有效期限
                return true;
                #endregion
            }
            else
            {
                #region Token 已經失效了，需要更新
                var fooResult = await refreshTokenService.GetAsync();
                if (fooResult.Status != true)
                {
                    await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                    return false;
                }
                systemStatusService.SingleItem = appStatus.SystemStatus;
                systemStatusService.SingleItem.IsLogin = true;
                systemStatusService.SingleItem.LoginedTime = DateTime.Now;
                systemStatusService.SingleItem.Token = refreshTokenService.SingleItem.Token;
                systemStatusService.SingleItem.RefreshToken = refreshTokenService.SingleItem.RefreshToken;
                systemStatusService.SingleItem.TokenExpireMinutes = refreshTokenService.SingleItem.TokenExpireMinutes;
                systemStatusService.SingleItem.RefreshTokenExpireDays = refreshTokenService.SingleItem.RefreshTokenExpireDays;
                systemStatusService.SingleItem.SetExpireDatetime();
                #endregion
            }

            //await systemStatusService.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusService, appStatus);

            return true;
        }
    }
}
