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
    using Plugin.Connectivity;
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
        private readonly MyUserService myUserService;
        private readonly RecordCacheHelper recordCacheHelper;
        private readonly AppStatus appStatus;

        public SplashPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            SystemStatusService systemStatusService, SystemEnvironmentsService systemEnvironmentsService,
            MyUserService myUserService,
            RecordCacheHelper recordCacheHelper, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.systemStatusService = systemStatusService;
            this.systemEnvironmentsService = systemEnvironmentsService;
            this.myUserService = myUserService;
            this.recordCacheHelper = recordCacheHelper;
            this.appStatus = appStatus;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            #region 確認網路已經連線
            //if (await UtilityHelper.CanConnectRemoteHostService() == false)
            //{
            //    await dialogService.DisplayAlertAsync("警告", "無網路連線可用 或者 無法連線到遠端主機，請檢查網路狀態與主機服務是否可以使用", "確定");
            //    return;
            //}
            #endregion

            #region 讀取相關定義資料
            using (IProgressDialog fooIProgressDialog =
                UserDialogs.Instance.Loading($"請稍後，更新資料中...",
                null, null, true, MaskType.Clear))
            {
                await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusService, appStatus);

            }
            #endregion

            if (appStatus.SystemStatus.IsLogin == false)
            {
                // 使用者尚未成功登入，切換到登入頁面
                await navigationService.NavigateAsync("/LoginPage");
                return;
            }

            #region 使用者已經成功登入了，接下來要更新相關資料
            using (IProgressDialog fooIProgressDialog =
                UserDialogs.Instance.Loading($"請稍後，更新資料中...",
                null, null, true, MaskType.Clear))
            {
                #region 取得 使用者清單
                fooIProgressDialog.Title = "請稍後，取得 使用者清單";
                await myUserService.ReadFromFileAsync();
                var fooResult = await myUserService.GetAsync();
                if (fooResult.Status == true)
                {
                    await myUserService.WriteToFileAsync();
                }
                #endregion
            }

            // 使用者尚已經功登入，切換到首頁頁面
            await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            #endregion
        }

    }
}
