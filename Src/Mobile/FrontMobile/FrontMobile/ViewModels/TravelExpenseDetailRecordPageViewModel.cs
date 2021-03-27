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
    public class TravelExpenseDetailRecordPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly TravelExpenseDetailService travelExpenseDetailService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public TravelExpenseDetailDto SelectedItem { get; set; }
        public string CrudAction { get; set; }
        public bool ShowDeleteButton { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public TravelExpenseDto MasterItem { get; set; }

        public TravelExpenseDetailRecordPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
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

            #region 新增 儲存 按鈕命令
            SaveCommand = new DelegateCommand(async () =>
            {
                #region 進行資料完整性檢查
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
                        #region 新增 差旅費用明細紀錄
                        fooIProgressDialog.Title = "請稍後，新增 差旅費用明細紀錄";
                        SelectedItem.Id = 0;
                        apiResult = await travelExpenseDetailService.PostAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"差旅費用明細紀錄 已經新增");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"差旅費用明細紀錄 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }
                    else
                    {
                        #region 儲存 差旅費用明細紀錄
                        fooIProgressDialog.Title = "請稍後，儲存 差旅費用明細紀錄";
                        apiResult = await travelExpenseDetailService.PutAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"差旅費用明細紀錄 已經儲存");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"差旅費用明細紀錄 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }

                    #region 取得 差旅費用明細紀錄
                    fooIProgressDialog.Title = "請稍後，取得 差旅費用明細紀錄";
                    apiResult = await travelExpenseDetailService.GetAsync(MasterItem.Id);
                    if (apiResult.Status == true)
                    {
                        await travelExpenseDetailService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得 差旅費用明細紀錄 失敗:{apiResult.Message}", "確定");
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

                    #region 刪除 差旅費用明細紀錄
                    fooIProgressDialog.Title = "請稍後，刪除 差旅費用明細紀錄";
                    apiResult = await travelExpenseDetailService.DeleteAsync(SelectedItem);
                    if (apiResult.Status == true)
                    {
                        ToastHelper.ShowToast($"差旅費用明細紀錄 已經刪除");

                        NavigationParameters paras = new NavigationParameters();
                        paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                        await navigationService.GoBackAsync(paras);
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"差旅費用明細紀錄 刪除失敗:{apiResult.Message}", "確定");
                    }
                    #endregion

                    #region 取得 差旅費用明細紀錄
                    fooIProgressDialog.Title = "請稍後，取得 差旅費用明細紀錄";
                    apiResult = await travelExpenseDetailService.GetAsync(MasterItem.Id);
                    if (apiResult.Status == true)
                    {
                        await travelExpenseDetailService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得 差旅費用明細紀錄 失敗:{apiResult.Message}", "確定");
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

            MasterItem = parameters.GetValue<TravelExpenseDto>(MagicStringHelper.MasterRecordActionName);
            var fooObject = parameters.GetValue<TravelExpenseDetailDto>(MagicStringHelper.CurrentSelectdItemParameterName);
            SelectedItem = fooObject;
            CrudAction = parameters.GetValue<string>(MagicStringHelper.CrudActionName);
            ShowDeleteButton = true;
            if (CrudAction == MagicStringHelper.CrudAddAction)
            {
                ShowDeleteButton = false;
            }
        }

        async Task LoadPickerSourceAsync()
        {
            await Task.Yield();
            return;
        }

    }
}
