using Acr.UserDialogs;
using Business.DataModel;
using Business.Services;
using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ManagerHelps
{
    public class LogoutCleanHelper
    {
        private readonly IPageDialogService dialogService;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly ExceptionRecordsManager exceptionRecordsManager;
        private readonly AppExceptionsManager appExceptionsManager;

        public LogoutCleanHelper(IPageDialogService dialogService, 
            SystemEnvironmentsManager systemEnvironmentsManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus, RefreshTokenManager refreshTokenManager,
            ExceptionRecordsManager exceptionRecordsManager, AppExceptionsManager appExceptionsManager)
        {
            this.dialogService = dialogService;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.refreshTokenManager = refreshTokenManager;
            this.exceptionRecordsManager = exceptionRecordsManager;
            this.appExceptionsManager = appExceptionsManager;
        }

        public async Task<bool> LogoutCleanAsync(IProgressDialog progressDialog)
        {
            APIResult fooAPIResult;
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
            if (fooRefreshTokenResult == false)
            {
                return false;
            }

            progressDialog.Title = $"回報例外異常資料中";

            #region 上傳例外異常
            await appExceptionsManager.ReadFromFileAsync();
            if (appExceptionsManager.Items.Count > 0)
            {
                await appExceptionsManager.ReadFromFileAsync();
                var fooResult = await exceptionRecordsManager.PostAsync(appExceptionsManager.Items);
                if (fooResult.Status == true)
                {
                    exceptionRecordsManager.Items.Clear();
                    await exceptionRecordsManager.WriteToFileAsync();
                }
                else
                {
                    await dialogService.DisplayAlertAsync("回報例外異常資料中 發生錯誤", fooResult.Message, "確定");
                    return false;
                }
            }
            #endregion

            return true;
        }
    }
}
