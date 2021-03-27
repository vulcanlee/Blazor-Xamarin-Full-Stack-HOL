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
    using CommonLibrary.Helpers.Utilities;
    using DataTransferObject.DTOs;
    using FrontMobile.Models;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class WorkingLogRecordPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly WorkingLogService workingLogService;
        private readonly MyUserService myUserService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
   
        public WorkingLogDto SelectedItem { get; set; }
        public string CrudAction { get; set; }
        public ObservableCollection<PickerItemModel> MyUserItemsSource { get; set; } = new ObservableCollection<PickerItemModel>();
        public PickerItemModel MyUserSelectedItem { get; set; }
        public bool ShowDeleteButton { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public WorkingLogRecordPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            WorkingLogService workingLogService,
            MyUserService myUserService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.workingLogService = workingLogService;
            this.myUserService = myUserService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 新增 儲存 按鈕命令
            SaveCommand = new DelegateCommand(async () =>
            {
                #region 進行資料完整性檢查
                SelectedItem.CombineDate();
                var checkResult = SelectedItem.Validation();
                if (!string.IsNullOrEmpty(checkResult))
                {
                    await dialogService.DisplayAlertAsync("錯誤", $"請檢查並且修正錯誤{Environment.NewLine}{Environment.NewLine}" +
                        $"{checkResult}", "確定");
                    return;
                }
                #endregion

                #region 進行記錄儲存
                APIResult apiResult = new APIResult();
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，儲存資料中...",
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

                    if (CrudAction == MagicStringHelper.CrudAddAction)
                    {
                        #region 新增 工作日誌表單
                        fooIProgressDialog.Title = "請稍後，新增 工作日誌表單";
                        SelectedItem.Id = 0;
                        apiResult = await workingLogService.PostAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"工作日誌表單 已經新增");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"工作日誌表單 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }
                    else
                    {
                        #region 儲存 工作日誌表單
                        fooIProgressDialog.Title = "請稍後，儲存 工作日誌表單";
                        apiResult = await workingLogService.PutAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"工作日誌表單 已經儲存");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"工作日誌表單 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }

                    #region 取得 工作日誌表單
                    fooIProgressDialog.Title = "請稍後，取得 工作日誌表單";
                    apiResult = await workingLogService.GetAsync();
                    if (apiResult.Status == true)
                    {
                        await workingLogService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得 工作日誌表單 失敗:{apiResult.Message}", "確定");
                    }
                    #endregion
                }
                #endregion
            });
            #endregion

            #region 刪除 按鈕命令
            DeleteCommand = new DelegateCommand(async () =>
            {
                #region 進行記錄刪除
                var confirm = await dialogService.DisplayAlertAsync(
                    "警告", "是否要刪除這筆紀錄?", "確定", "取消");
                if (confirm == false)
                {
                    return;
                }

                APIResult apiResult = new APIResult();
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，刪除資料中...",
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

                    #region 刪除 工作日誌表單
                    fooIProgressDialog.Title = "請稍後，刪除 工作日誌表單";
                    apiResult = await workingLogService.DeleteAsync(SelectedItem);
                    if (apiResult.Status == true)
                    {
                        ToastHelper.ShowToast($"工作日誌表單 已經刪除");

                        NavigationParameters paras = new NavigationParameters();
                        paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                        await navigationService.GoBackAsync(paras);
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"工作日誌表單 刪除失敗:{apiResult.Message}", "確定");
                    }
                    #endregion

                    #region 取得 工作日誌表單
                    fooIProgressDialog.Title = "請稍後，取得 工作日誌表單";
                    apiResult = await workingLogService.GetAsync();
                    if (apiResult.Status == true)
                    {
                        await workingLogService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得請假單 失敗:{apiResult.Message}", "確定");
                    }
                    #endregion
                }
                #endregion
            });
            #endregion
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadPickerSourceAsync();

            var fooObject = parameters.GetValue<WorkingLogDto>(MagicStringHelper.CurrentSelectdItemParameterName);
            fooObject.SetDate();
            SelectedItem = fooObject;
            CrudAction = parameters.GetValue<string>(MagicStringHelper.CrudActionName);
            ShowDeleteButton = true;
            if (CrudAction == MagicStringHelper.CrudAddAction)
            {
                ShowDeleteButton = false;
            }

            #region 進行選單初始化
            #region 申請人
            if (SelectedItem.MyUserId >= 0)
            {
                MyUserSelectedItem = MyUserItemsSource
                    .FirstOrDefault(x => x.Id == SelectedItem.MyUserId);
            }
            #endregion
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

        async Task LoadPickerSourceAsync()
        {
            PickerItemModel pItem = new PickerItemModel();
            #region 讀取使用者
            await myUserService.ReadFromFileAsync();
            MyUserItemsSource.Clear();
            foreach (var item in myUserService.Items)
            {
                pItem = new PickerItemModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                };
                MyUserItemsSource.Add(pItem);
            }
            #endregion
        }
    }
}
