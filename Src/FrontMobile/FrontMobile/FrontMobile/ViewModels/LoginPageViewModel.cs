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
    using DataTransferObject.DTOs;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class LoginPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Account { get; set; }
        public string Password { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand<string> SwitchUserCommand { get; set; }

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LoginService loginService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        private readonly RecordCacheHelper recordCacheHelper;

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LoginService loginService, SystemStatusService systemStatusService,
            AppStatus appStatus, RecordCacheHelper recordCacheHelper)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginService = loginService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
            this.recordCacheHelper = recordCacheHelper;

            #region 登入按鈕命令
            LoginCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，使用者登入驗證中...", null, null, true, MaskType.Black))
                {
                    LoginRequestDto loginRequestDTO = new LoginRequestDto()
                    {
                        Account = Account,
                        Password = Password,
                    };
                    var fooResult = await LoginUpdateTokenHelper.UserLoginAsync(dialogService, loginService, systemStatusService,
                        loginRequestDTO, appStatus);
                    if (fooResult == false)
                    {
                        //await dialogService.DisplayAlertAsync("登入驗證失敗", "登入成功", "OK");
                        return;
                    }
                    await recordCacheHelper.RefreshAsync(fooIProgressDialog);
                }

                await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            });
            #endregion

            #region 切換使用者
            SwitchUserCommand = new DelegateCommand<string>(x =>
            {
                Account = x;
            });
            #endregion
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
#if DEBUG
            Account = "user1";
            Password = "pw";
#endif
            await systemStatusService.ReadFromFileAsync();
        }

    }
}
