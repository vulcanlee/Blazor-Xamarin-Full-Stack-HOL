using Prism.Commands;
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
    using Business.Helpers.ServiceHelps;
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
        private readonly SystemStatusService systemStatusService;
        private readonly SystemEnvironmentsService systemEnvironmentsService;
        private readonly RecordCacheHelper recordCacheHelper;
        private readonly AppStatus appStatus;
        private readonly ExceptionRecordsService exceptionRecordsService;
        private readonly AppExceptionsService appExceptionsService;
        private readonly LeaveCategoryService leaveCategoryService;
        private readonly OnCallPhoneService onCallPhoneService;

        public SplashPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            SystemStatusService systemStatusService, SystemEnvironmentsService systemEnvironmentsService,
            RecordCacheHelper recordCacheHelper, AppStatus appStatus,
            ExceptionRecordsService exceptionRecordsService, AppExceptionsService appExceptionsService,
            LeaveCategoryService leaveCategoryService, OnCallPhoneService onCallPhoneService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.systemStatusService = systemStatusService;
            this.systemEnvironmentsService = systemEnvironmentsService;
            this.recordCacheHelper = recordCacheHelper;
            this.appStatus = appStatus;
            this.exceptionRecordsService = exceptionRecordsService;
            this.appExceptionsService = appExceptionsService;
            this.leaveCategoryService = leaveCategoryService;
            this.onCallPhoneService = onCallPhoneService;
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

            #region 讀取相關定義資料
            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusService, appStatus);
                #region 取得請假假別
                fooIProgressDialog.Title = "請稍後，取得 連絡電話本";
                await onCallPhoneService.ReadFromFileAsync();
                var fooResult = await onCallPhoneService.GetAsync();
                if (fooResult.Status == true)
                {
                    await onCallPhoneService.WriteToFileAsync();
                }
                #endregion

                #region 取得請假假別
                fooIProgressDialog.Title = "請稍後，取得 請假假別";
                await leaveCategoryService.ReadFromFileAsync();
                fooResult = await leaveCategoryService.GetAsync();
                if (fooResult.Status == true)
                {
                    await leaveCategoryService.WriteToFileAsync();
                }
                #endregion

                #region 上傳例外異常
                fooIProgressDialog.Title = "請稍後，上傳例外異常";
                await appExceptionsService.ReadFromFileAsync();
                if (appExceptionsService.Items.Count > 0)
                {
                    await appExceptionsService.ReadFromFileAsync();
                    fooResult = await exceptionRecordsService.PostAsync(appExceptionsService.Items);
                    if (fooResult.Status == true)
                    {
                        appExceptionsService.Items.Clear();
                        await appExceptionsService.WriteToFileAsync();
                    }
                }
                #endregion
            }
            #endregion

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
