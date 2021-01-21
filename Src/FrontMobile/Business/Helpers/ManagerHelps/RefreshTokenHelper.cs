using Business.DataModel;
using Business.Services;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ManagerHelps
{
    public class RefreshTokenHelper
    {
        public static async Task<bool> CheckAndRefreshToken(IPageDialogService dialogService,
            RefreshTokenManager refreshTokenManager, SystemStatusManager systemStatusManager,
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
                var fooResult = await refreshTokenManager.GetAsync();
                if (fooResult.Status != true)
                {
                    await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                    return false;
                }
                systemStatusManager.SingleItem = appStatus.SystemStatus;
                systemStatusManager.SingleItem.IsLogin = true;
                systemStatusManager.SingleItem.LoginedTime = DateTime.Now;
                systemStatusManager.SingleItem.Token = refreshTokenManager.SingleItem.Token;
                systemStatusManager.SingleItem.RefreshToken = refreshTokenManager.SingleItem.RefreshToken;
                systemStatusManager.SingleItem.TokenExpireMinutes = refreshTokenManager.SingleItem.TokenExpireMinutes;
                systemStatusManager.SingleItem.RefreshTokenExpireDays = refreshTokenManager.SingleItem.RefreshTokenExpireDays;
                systemStatusManager.SingleItem.SetExpireDatetime();
                #endregion
            }

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
    }
}
