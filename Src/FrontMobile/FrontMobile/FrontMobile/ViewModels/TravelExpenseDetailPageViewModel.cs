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
    public class TravelExpenseDetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly TravelExpenseDetailService travelExpenseDetailService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public ObservableCollection<TravelExpenseDetailDto> DataItems { get; set; } = new ObservableCollection<TravelExpenseDetailDto>();
        public bool IsRefresh { get; set; }
        public TravelExpenseDetailDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public TravelExpenseDto MasterItem { get; set; }

        public TravelExpenseDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            TravelExpenseDetailService travelExpenseDetailService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.travelExpenseDetailService = travelExpenseDetailService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 新增紀錄
            AddCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = new TravelExpenseDetailDto();
                fooObject.TravelExpenseId = MasterItem.Id;
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudAddAction);
                paras.Add(MagicStringHelper.MasterRecordActionName, MasterItem);
                await navigationService.NavigateAsync("TravelExpenseDetailRecordPage", paras);
            });
            #endregion

            #region 點選某筆紀錄觸發命令
            ItemTappedCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = SelectedItem.Clone();
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudEditAction);
                paras.Add(MagicStringHelper.MasterRecordActionName, MasterItem);
                await navigationService.NavigateAsync("TravelExpenseDetailRecordPage", paras);
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
                MasterItem = parameters.GetValue<TravelExpenseDto>(MagicStringHelper.MasterRecordActionName);
                await RefreshData();
            }
            else
            {
                string CrudAction = parameters.GetValue<string>(MagicStringHelper.CrudActionName);
                if (CrudAction == MagicStringHelper.CrudRefreshAction)
                {
                    await ReloadData();
                }
            }
        }

        async Task RefreshData()
        {
            await travelExpenseDetailService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in travelExpenseDetailService.Items)
            {
                DataItems.Add(item);
            }
        }
        async Task ReloadData()
        {
            #region 讀取 相關定義資料
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

                #region 取得 差旅費用明細紀錄
                fooIProgressDialog.Title = "請稍後，取得 差旅費用明細紀錄";
                var apiResultssss = await travelExpenseDetailService.GetAsync(MasterItem.Id);
                if (apiResultssss.Status == true)
                {
                    await travelExpenseDetailService.WriteToFileAsync();
                    await RefreshData();
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
