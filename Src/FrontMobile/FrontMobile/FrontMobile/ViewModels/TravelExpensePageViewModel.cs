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
    using FrontMobile.Models;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class TravelExpensePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly TravelExpenseService travelExpenseService;
        private readonly MyUserService myUserService;
        private readonly TravelExpenseDetailService travelExpenseDetailService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public ObservableCollection<PickerItemModel> MyUserItemsSource { get; set; } = new ObservableCollection<PickerItemModel>();
        public PickerItemModel MyUserSelectedItem { get; set; }
        public ObservableCollection<TravelExpenseDto> DataItems { get; set; } = new ObservableCollection<TravelExpenseDto>();
        public bool IsRefresh { get; set; }
        public TravelExpenseDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand<TravelExpenseDto> ShowDetailCommand { get; set; }

        public TravelExpensePageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            TravelExpenseService travelExpenseService,
            MyUserService myUserService, TravelExpenseDetailService travelExpenseDetailService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.travelExpenseService = travelExpenseService;
            this.myUserService = myUserService;
            this.travelExpenseDetailService = travelExpenseDetailService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 新增紀錄
            AddCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = new TravelExpenseDto();
                fooObject.ApplyDate = DateTime.Now.Date;
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudAddAction);
                await navigationService.NavigateAsync("TravelExpenseRecordPage", paras);
            });
            #endregion

            #region 點選某筆紀錄觸發命令
            ItemTappedCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = SelectedItem.Clone();
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudEditAction);
                await navigationService.NavigateAsync("TravelExpenseRecordPage", paras);
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

            #region 顯示明細頁面
            ShowDetailCommand = new DelegateCommand<TravelExpenseDto>(async x =>
            {
                #region 讀取該筆明細清單
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
                    var apiResultssss = await travelExpenseDetailService.GetAsync(x.Id);
                    if (apiResultssss.Status == true)
                    {
                        await travelExpenseDetailService.WriteToFileAsync();

                        NavigationParameters paras = new NavigationParameters();
                        paras.Add(MagicStringHelper.MasterRecordActionName, x);
                        await navigationService.NavigateAsync("TravelExpenseDetailPage", paras);
                    }
                    else
                    {
                        await DialogHelper.ShowAPIResultIsFailureMessage(dialogService, apiResultssss);
                        return;
                    }
                    #endregion
                }
                #endregion

            });
            #endregion
        }

        #region Fody 自動綁定事件
        public void OnMyUserSelectedItemChanged()
        {
            if (MyUserSelectedItem != null)
            {
                SelectedItem.MyUserId = MyUserSelectedItem.Id;
            }
            else
            {
                SelectedItem.MyUserId = -1;
            }
        }
        #endregion

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
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
            await travelExpenseService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in travelExpenseService.Items)
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

                #region 取得人員清單
                fooIProgressDialog.Title = "請稍後，取得人員清單";
                var apiResultssss = await myUserService.GetAsync();
                if (apiResultssss.Status == true)
                {
                    await myUserService.WriteToFileAsync();
                }
                else
                {
                    await DialogHelper.ShowAPIResultIsFailureMessage(dialogService, apiResultssss);
                    return;
                }
                #endregion
                #region 取得 差旅費用表單
                fooIProgressDialog.Title = "請稍後，取得 差旅費用表單";
                apiResultssss = await travelExpenseService.GetAsync();
                if (apiResultssss.Status == true)
                {
                    await travelExpenseService.WriteToFileAsync();
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
