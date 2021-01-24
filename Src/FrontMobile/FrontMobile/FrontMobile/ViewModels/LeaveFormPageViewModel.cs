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
    using CommonLibrary.Helpers.Magics;
    using DataTransferObject.DTOs;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class LeaveFormPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly LeaveFormService leaveFormService;
        private readonly LeaveCategoryService leaveCategoryService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        public ObservableCollection<LeaveCategoryDto> PickerDataItems { get; set; } = new ObservableCollection<LeaveCategoryDto>();
        public ObservableCollection<LeaveFormDto> DataItems { get; set; } = new ObservableCollection<LeaveFormDto>();
        public bool IsRefresh { get; set; }
        public LeaveFormDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemSelectedCommand { get; set; }

        public LeaveFormPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LeaveFormService leaveFormService, LeaveCategoryService leaveCategoryService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.leaveFormService = leaveFormService;
            this.leaveCategoryService = leaveCategoryService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 點選某筆紀錄觸發命令
            ItemSelectedCommand = new DelegateCommand(async() =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = SelectedItem.Clone();
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                await navigationService.NavigateAsync("LeaveFormDetailPage", paras);
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
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                await RefreshData();
            }
        }
        async Task RefreshData()
        {
            await leaveFormService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in leaveFormService.Items)
            {
                DataItems.Add(item);
            }
        }
        async Task ReloadData()
        {
            #region 讀取相關定義資料
            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusService, appStatus);
                #region 取得請假假別
                fooIProgressDialog.Title = "請稍後，取得請假單";
                var fooResult = await leaveFormService.GetAsync();
                if (fooResult.Status == true)
                {
                    await leaveFormService.WriteToFileAsync();
                    await RefreshData();
                }
                #endregion
            }
            #endregion
        }
    }
}
