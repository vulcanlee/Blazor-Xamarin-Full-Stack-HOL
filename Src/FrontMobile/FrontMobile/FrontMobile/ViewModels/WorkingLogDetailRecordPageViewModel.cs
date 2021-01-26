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
    public class WorkingLogDetailRecordPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly WorkingLogDetailService workingLogDetailService;
        private readonly ProjectService projectService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public WorkingLogDetailDto SelectedItem { get; set; }
        public string CrudAction { get; set; }
        public ObservableCollection<PickerItemModel> ProjectItemsSource { get; set; } = new ObservableCollection<PickerItemModel>();
        public PickerItemModel ProjectSelectedItem { get; set; }
        public bool ShowDeleteButton { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public WorkingLogDto MasterItem { get; set; }

        public WorkingLogDetailRecordPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            WorkingLogDetailService workingLogDetailService, ProjectService projectService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.workingLogDetailService = workingLogDetailService;
            this.projectService = projectService;
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
                        #region 新增 工作日誌明細紀錄
                        fooIProgressDialog.Title = "請稍後，新增 工作日誌明細紀錄";
                        SelectedItem.Id = 0;
                        apiResult = await workingLogDetailService.PostAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"工作日誌明細紀錄 已經新增");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"工作日誌明細紀錄 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }
                    else
                    {
                        #region 儲存 工作日誌明細紀錄
                        fooIProgressDialog.Title = "請稍後，儲存 工作日誌明細紀錄";
                        apiResult = await workingLogDetailService.PutAsync(SelectedItem);
                        if (apiResult.Status == true)
                        {
                            ToastHelper.ShowToast($"工作日誌明細紀錄 已經儲存");

                            NavigationParameters paras = new NavigationParameters();
                            paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                            await navigationService.GoBackAsync(paras);
                        }
                        else
                        {
                            await dialogService.DisplayAlertAsync("錯誤", $"工作日誌明細紀錄 儲存失敗:{apiResult.Message}", "確定");
                        }
                        #endregion
                    }

                    #region 取得 工作日誌明細紀錄
                    fooIProgressDialog.Title = "請稍後，取得 工作日誌明細紀錄";
                    apiResult = await workingLogDetailService.GetAsync(MasterItem.Id);
                    if (apiResult.Status == true)
                    {
                        await workingLogDetailService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得 工作日誌明細紀錄 失敗:{apiResult.Message}", "確定");
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

                    #region 刪除 工作日誌明細紀錄
                    fooIProgressDialog.Title = "請稍後，刪除 工作日誌明細紀錄";
                    apiResult = await workingLogDetailService.DeleteAsync(SelectedItem);
                    if (apiResult.Status == true)
                    {
                        ToastHelper.ShowToast($"工作日誌明細紀錄 已經刪除");

                        NavigationParameters paras = new NavigationParameters();
                        paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudRefreshAction);
                        await navigationService.GoBackAsync(paras);
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"工作日誌明細紀錄 刪除失敗:{apiResult.Message}", "確定");
                    }
                    #endregion

                    #region 取得 工作日誌明細紀錄
                    fooIProgressDialog.Title = "請稍後，取得 工作日誌明細紀錄";
                    apiResult = await workingLogDetailService.GetAsync(MasterItem.Id);
                    if (apiResult.Status == true)
                    {
                        await workingLogDetailService.WriteToFileAsync();
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"取得 工作日誌明細紀錄 失敗:{apiResult.Message}", "確定");
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

            MasterItem = parameters.GetValue<WorkingLogDto>(MagicStringHelper.MasterRecordActionName);
            var fooObject = parameters.GetValue<WorkingLogDetailDto>(MagicStringHelper.CurrentSelectdItemParameterName);
            SelectedItem = fooObject;
            CrudAction = parameters.GetValue<string>(MagicStringHelper.CrudActionName);
            ShowDeleteButton = true;
            if (CrudAction == MagicStringHelper.CrudAddAction)
            {
                ShowDeleteButton = false;
            }

            #region 進行選單初始化
            #region 專案
            if (SelectedItem.ProjectId >= 0)
            {
                ProjectSelectedItem = ProjectItemsSource
                    .FirstOrDefault(x => x.Id == SelectedItem.ProjectId);
            }
            #endregion
            #endregion
        }


        #region Fody 自動綁定事件
        public void OnProjectSelectedItemChanged()
        {
            if (ProjectSelectedItem != null)
            {
                SelectedItem.ProjectId = ProjectSelectedItem.Id;
            }
            else
            {
                SelectedItem.ProjectId = -1;
            }
        }
        #endregion

        async Task LoadPickerSourceAsync()
        {
            PickerItemModel pItem = new PickerItemModel();
            #region 讀取 專案
            await projectService.ReadFromFileAsync();
            ProjectItemsSource.Clear();
            foreach (var item in projectService.Items)
            {
                pItem = new PickerItemModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                };
                ProjectItemsSource.Add(pItem);
            }
            #endregion
        }

    }
}
