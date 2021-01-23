﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontMobile.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using Business.DataModel;
    using Business.Helpers.ManagerHelps;
    using Business.Services;
    using CommonLibrary.Helpers.Utilities;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Xamarin.CommunityToolkit.UI.Views.Options;
    using Xamarin.Forms;

    public class SplashPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly SystemStatusManager systemStatusManager;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly RecordCacheHelper recordCacheHelper;
        private readonly AppStatus appStatus;
        private readonly ExceptionRecordsManager exceptionRecordsManager;
        private readonly AppExceptionsManager appExceptionsManager;
        private readonly LeaveCategoryManager leaveCategoryManager;

        public SplashPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            SystemStatusManager systemStatusManager, SystemEnvironmentsManager systemEnvironmentsManager,
            RecordCacheHelper recordCacheHelper, AppStatus appStatus,
            ExceptionRecordsManager exceptionRecordsManager, AppExceptionsManager appExceptionsManager,
            LeaveCategoryManager leaveCategoryManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.systemStatusManager = systemStatusManager;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.recordCacheHelper = recordCacheHelper;
            this.appStatus = appStatus;
            this.exceptionRecordsManager = exceptionRecordsManager;
            this.appExceptionsManager = appExceptionsManager;
            this.leaveCategoryManager = leaveCategoryManager;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            #region 確認網路已經連線
            if (UtilityHelper.IsConnected() == false)
            {
                await dialogService.DisplayAlertAsync("警告", "無網路連線可用，請檢查網路狀態", "確定");
                return;
            }
            #endregion

            #region Toast
            IUserDialogs dialogs = UserDialogs.Instance;
            ToastConfig.DefaultBackgroundColor = System.Drawing.Color.AliceBlue;
            ToastConfig.DefaultMessageTextColor = System.Drawing.Color.Red;
            ToastConfig.DefaultActionTextColor = System.Drawing.Color.DarkRed;
            //var bgColor = FromHex(this.BackgroundColor);
            //var msgColor = FromHex(this.MessageTextColor);
            //var actionColor = FromHex(this.ActionTextColor);

            dialogs.Toast(new ToastConfig($"已經有最新版本推出")
                //.SetBackgroundColor(bgColor)
                //.SetMessageTextColor(msgColor)
                .SetDuration(TimeSpan.FromSeconds(3))
                .SetPosition( ToastPosition.Bottom)
                //.SetIcon(icon)
                .SetAction(x => x
                    .SetText("this.ActionText")
                    //.SetTextColor(actionColor)
                    .SetAction(() => dialogs.Alert("You clicked the primary toast button"))
                )
            );

            #endregion

            #region 讀取相關定義資料
            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusManager, appStatus);
                #region 取得請假假別
                fooIProgressDialog.Title = "請稍後，取得請假假別";
                    await leaveCategoryManager.ReadFromFileAsync();
                    var fooResult = await leaveCategoryManager.GetAsync();
                    if (fooResult.Status == true)
                    {
                        await leaveCategoryManager.WriteToFileAsync();
                    }
                #endregion

                #region 上傳例外異常
                fooIProgressDialog.Title = "請稍後，上傳例外異常";
                await appExceptionsManager.ReadFromFileAsync();
                if (appExceptionsManager.Items.Count > 0)
                {
                    await appExceptionsManager.ReadFromFileAsync();
                    fooResult = await exceptionRecordsManager.PostAsync(appExceptionsManager.Items);
                    if (fooResult.Status == true)
                    {
                        appExceptionsManager.Items.Clear();
                        await appExceptionsManager.WriteToFileAsync();
                    }
                }
                #endregion
            }
            #endregion

            await Task.Delay(3000);
            if (appStatus.SystemStatus.IsLogin == false)
            {
                // 使用者尚未成功登入，切換到登入頁面
                await navigationService.NavigateAsync("/LoginPage");
                return;
            }

            #region 使用者已經成功登入了，接下來要更新相關資料
            //using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，上傳例外異常資料中...", null, null, true, MaskType.Black))
            //{
            //    await recordCacheHelper.RefreshAsync(fooIProgressDialog);

            //}

            // 使用者尚已經功登入，切換到首頁頁面
            await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            #endregion
        }

    }
}
