using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontMobile.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using Business.DataModel;
    using Business.Helpers.ServiceHelps;
    using Business.Services;
    using DataTransferObject.DTOs;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Xamarin.Essentials;

    public class OnCallPhonePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly OnCallPhoneService onCallPhoneService;
        private readonly MyUserService myUserService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public ObservableCollection<OnCallPhoneDto> DataItems { get; set; } = new ObservableCollection<OnCallPhoneDto>();
        public bool IsRefresh { get; set; }
        public OnCallPhoneDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemTappedCommand { get; set; }

        public OnCallPhonePageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            OnCallPhoneService onCallPhoneService, 
            MyUserService myUserService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.onCallPhoneService = onCallPhoneService;
            this.myUserService = myUserService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 點選某筆紀錄觸發命令
            ItemTappedCommand = new DelegateCommand(async () =>
            {
                try
                {
                    PhoneDialer.Open(SelectedItem.PhoneNumber);
                }
                catch (Exception ex)
                {
                    await dialogService.DisplayAlertAsync("錯誤", $"發生例外異常:{ex.Message}", "確定");
                }
            });
            #endregion

            #region 更新遠端紀錄命令
            RefreshCommand = new DelegateCommand(async () =>
            {
                IsRefresh = true;
                await ReloadData();
                IsRefresh = false;
            });
            #endregion
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await RefreshData();
        }
        async Task RefreshData()
        {
            await onCallPhoneService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in onCallPhoneService.Items)
            {
                DataItems.Add(item);
            }
        }
        async Task ReloadData()
        {
            #region 讀取相關定義資料
            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...",
                null, null, true, MaskType.Black))
            {
                await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusService, appStatus);
                #region 檢查 Access Token 是否還可以使用
                bool refreshTokenResult = await RefreshTokenHelper
                    .CheckAndRefreshToken(dialogService, refreshTokenService,
                    systemStatusService, appStatus);
                if (refreshTokenResult == false)
                {
                    return;
                }
                #endregion

                #region 取得 連絡電話本
                fooIProgressDialog.Title = "請稍後，取得 連絡電話本";
                var apiResultssss = await onCallPhoneService.GetAsync();
                if (apiResultssss.Status == true)
                {
                    await onCallPhoneService.WriteToFileAsync();
                }
                else
                {
                    await DialogHelper.ShowAPIResultIsFailureMessage(dialogService, apiResultssss);
                    return;
                }
                #endregion
            }
            #endregion
        }

    }
}
