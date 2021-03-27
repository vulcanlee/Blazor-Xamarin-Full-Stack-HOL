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
        private readonly MyUserService myUserService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        private readonly RecordCacheHelper recordCacheHelper;
        public bool ShowAccountValidationErrorMessage { get; set; }
        public bool ShowPasswordValidationErrorMessage { get; set; }

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LoginService loginService, MyUserService myUserService, SystemStatusService systemStatusService,
            AppStatus appStatus, RecordCacheHelper recordCacheHelper)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginService = loginService;
            this.myUserService = myUserService;
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
                        await dialogService.DisplayAlertAsync("登入驗證失敗", "請重新輸入正確的帳號與密碼", "確定");
                        return;
                    }
                    await recordCacheHelper.RefreshAsync(fooIProgressDialog);
                    #region 取得 使用者清單
                    fooIProgressDialog.Title = "請稍後，取得 使用者清單";
                    await myUserService.ReadFromFileAsync();
                    APIResult apiResult = await myUserService.GetAsync();
                    if (apiResult.Status == true)
                    {
                        await myUserService.WriteToFileAsync();
                    }
                    #endregion
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

            OnAccountChanged();
            OnPasswordChanged();
        }

        #region Fody 自動綁定事件
        public void OnAccountChanged()
        {
            if (string.IsNullOrEmpty(Account))
            {
                ShowAccountValidationErrorMessage = true;
            }
            else
            {
                ShowAccountValidationErrorMessage = false;
            }
        }
        public void OnPasswordChanged()
        {
            if (string.IsNullOrEmpty(Account))
            {
                ShowPasswordValidationErrorMessage = true;
            }
            else
            {
                ShowPasswordValidationErrorMessage = false;
            }
        }
        #endregion
    }
}
