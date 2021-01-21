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
    public class MDPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LoginManager loginManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly RecordCacheHelper recordCacheHelper;
        private readonly LogoutCleanHelper logoutCleanHelper;

        public DelegateCommand LogoutCommand { get; set; }
        public MDPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager,
            AppStatus appStatus, RecordCacheHelper recordCacheHelper, 
            LogoutCleanHelper logoutCleanHelper)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginManager = loginManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.recordCacheHelper = recordCacheHelper;
            this.logoutCleanHelper = logoutCleanHelper;

            #region 登出命令
            LogoutCommand = new DelegateCommand(async () =>
            {
                var isLogout = await dialogService.DisplayAlertAsync("警告",
                    "你確定要登出嗎？", "確定", "取消");
                if(isLogout== true)
                {
                    using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                    {
                        await logoutCleanHelper.LogoutCleanAsync(fooIProgressDialog);
                        var fooResult = await LoginUpdateTokenHelper.UserLogoutAsync(dialogService, loginManager, systemStatusManager, appStatus);
                        if (fooResult == true)
                        {
                            await navigationService.NavigateAsync("/LoginPage");
                        }
                    }
                }
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
