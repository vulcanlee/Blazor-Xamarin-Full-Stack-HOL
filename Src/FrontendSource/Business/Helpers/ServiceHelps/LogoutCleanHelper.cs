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

        public LogoutCleanHelper(IPageDialogService dialogService, 
            SystemEnvironmentsService systemEnvironmentsService,
            SystemStatusService systemStatusService, AppStatus appStatus, RefreshTokenService refreshTokenService)
        {
            this.dialogService = dialogService;
            this.systemEnvironmentsService = systemEnvironmentsService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<bool> LogoutCleanAsync(IProgressDialog progressDialog)
        {
            APIResult fooAPIResult;
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper
                .CheckAndRefreshToken(dialogService, refreshTokenService, 
                systemStatusService, appStatus);
            if (fooRefreshTokenResult == false)
            {
                return false;
            }

            progressDialog.Title = $"回報例外異常資料中";

            return true;
        }
    }
}
