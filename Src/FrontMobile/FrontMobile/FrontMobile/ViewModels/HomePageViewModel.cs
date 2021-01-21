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
    using Business.Helpers.ManagerHelps;
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
        private readonly OnlyAdministratorManager onlyAdministratorManager;
        private readonly OnlyUserManager onlyUserManager;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly AppExceptionsManager appExceptionsManager;
        private readonly ExceptionRecordsManager exceptionRecordsManager;

        public string Message { get; set; }

        public HomePageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            OnlyAdministratorManager OnlyAdministratorManager, OnlyUserManager OnlyUserManager,
            RefreshTokenManager refreshTokenManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus,
            AppExceptionsManager appExceptionsManager, ExceptionRecordsManager exceptionRecordsManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            onlyAdministratorManager = OnlyAdministratorManager;
            onlyUserManager = OnlyUserManager;
            this.refreshTokenManager = refreshTokenManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.appExceptionsManager = appExceptionsManager;
            this.exceptionRecordsManager = exceptionRecordsManager;

            #region OnlyAdministratorCommand
            OnlyAdministratorCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，執行中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await OnlyAdministratorManager.GetAsync();
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
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await OnlyUserManager.GetAsync();
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
