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

namespace Business.Helpers.ServiceHelps
{
    public class LogoutCleanHelper
    {
        private readonly IPageDialogService dialogService;
        private readonly SystemEnvironmentsService systemEnvironmentsService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        private readonly RefreshTokenService refreshTokenService;
        private readonly ExceptionRecordsService exceptionRecordsService;
        private readonly AppExceptionsService appExceptionsService;

        public LogoutCleanHelper(IPageDialogService dialogService, 
            SystemEnvironmentsService systemEnvironmentsService,
            SystemStatusService systemStatusService, AppStatus appStatus, RefreshTokenService refreshTokenService,
            ExceptionRecordsService exceptionRecordsService, AppExceptionsService appExceptionsService)
        {
            this.dialogService = dialogService;
            this.systemEnvironmentsService = systemEnvironmentsService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
            this.refreshTokenService = refreshTokenService;
            this.exceptionRecordsService = exceptionRecordsService;
            this.appExceptionsService = appExceptionsService;
        }

        public async Task<bool> LogoutCleanAsync(IProgressDialog progressDialog)
        {
            APIResult fooAPIResult;
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenService, systemStatusService, appStatus);
            if (fooRefreshTokenResult == false)
            {
                return false;
            }

            progressDialog.Title = $"回報例外異常資料中";

            #region 上傳例外異常
            await appExceptionsService.ReadFromFileAsync();
            if (appExceptionsService.Items.Count > 0)
            {
                await appExceptionsService.ReadFromFileAsync();
                var fooResult = await exceptionRecordsService.PostAsync(appExceptionsService.Items);
                if (fooResult.Status == true)
                {
                    exceptionRecordsService.Items.Clear();
                    await exceptionRecordsService.WriteToFileAsync();
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
