using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontMobile.ViewModels
{
    using System.ComponentModel;
    using Acr.UserDialogs;
    using Business.DataModel;
    using Business.Helpers.ServiceHelps;
    using Business.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class HomePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand OnlyAdministratorCommand { get; set; }
        public DelegateCommand OnlyUserCommand { get; set; }
        public DelegateCommand ThrowExceptionrCommand { get; set; }
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly OnlyAdministratorService onlyAdministratorService;
        private readonly OnlyUserService onlyUserService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        private readonly AppExceptionsService appExceptionsService;
        private readonly ExceptionRecordsService exceptionRecordsService;

        public string Message { get; set; }

        public HomePageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            OnlyAdministratorService OnlyAdministratorService, OnlyUserService OnlyUserService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus,
            AppExceptionsService appExceptionsService, ExceptionRecordsService exceptionRecordsService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            onlyAdministratorService = OnlyAdministratorService;
            onlyUserService = OnlyUserService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
            this.appExceptionsService = appExceptionsService;
            this.exceptionRecordsService = exceptionRecordsService;

            #region OnlyAdministratorCommand
            OnlyAdministratorCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，執行中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenService, systemStatusService, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await OnlyAdministratorService.GetAsync();
                    if(fooResult.Status ==false)
                    {
                        Message = fooResult.Message;
                    }
                    else
                    {
                        Message = fooResult.Payload.ToString();
                    }
                }
            });
            #endregion
            #region OnlyUserCommand
            OnlyUserCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，執行中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenService, systemStatusService, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await OnlyUserService.GetAsync();
                    if (fooResult.Status == false)
                    {
                        Message = fooResult.Message;
                    }
                    else
                    {
                        Message = fooResult.Payload.ToString();
                    }
                }
            });
            #endregion
            #region 故意拋出例外
            ThrowExceptionrCommand = new DelegateCommand(async () =>
            {
                throw new Exception("魔鬼藏在細節中");
            });
            #endregion
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

    }
}
