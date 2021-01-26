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
    public class WorkingLogPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly WorkingLogService workingLogService;
        private readonly ProjectService projectService;
        private readonly MyUserService myUserService;
        private readonly WorkingLogDetailService workingLogDetailService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        public ObservableCollection<ProductDto> PickerDataItems { get; set; } = new ObservableCollection<ProductDto>();
        public ObservableCollection<WorkingLogDto> DataItems { get; set; } = new ObservableCollection<WorkingLogDto>();
        public bool IsRefresh { get; set; }
        public WorkingLogDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand<WorkingLogDto> ShowDetailCommand { get; set; }

        public WorkingLogPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            WorkingLogService workingLogService, ProjectService projectService,
            MyUserService myUserService, WorkingLogDetailService workingLogDetailService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.workingLogService = workingLogService;
            this.projectService = projectService;
            this.myUserService = myUserService;
            this.workingLogDetailService = workingLogDetailService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;

            #region 新增紀錄
            AddCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = new WorkingLogDto();
                fooObject.LogDate = DateTime.Now.Date;
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudAddAction);
                await navigationService.NavigateAsync("WorkingLogRecordPage", paras);
            });
            #endregion

            #region 點選某筆紀錄觸發命令
            ItemTappedCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = SelectedItem.Clone();
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudEditAction);
                await navigationService.NavigateAsync("WorkingLogRecordPage", paras);
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
            ShowDetailCommand = new DelegateCommand<WorkingLogDto>(async x =>
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

                    #region 取得 專案清單
                    fooIProgressDialog.Title = "請稍後，取得 專案清單";
                    var apiResultssss = await projectService.GetAsync();
                    if (apiResultssss.Status == true)
                    {
                        await projectService.WriteToFileAsync();
                    }
                    else
                    {
                        await DialogHelper.ShowAPIResultIsFailureMessage(dialogService, apiResultssss);
                        return;
                    }
                    #endregion

                    #region 取得 工作日誌明細
                    fooIProgressDialog.Title = "請稍後，取得 工作日誌明細";
                    apiResultssss = await workingLogDetailService.GetAsync(x.Id);
                    if (apiResultssss.Status == true)
                    {
                        await workingLogDetailService.WriteToFileAsync();

                        NavigationParameters paras = new NavigationParameters();
                        paras.Add(MagicStringHelper.MasterRecordActionName, x);
                        await navigationService.NavigateAsync("WorkingLogDetailPage", paras);
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
            await workingLogService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in workingLogService.Items)
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

                #region 取得 專案清單
                fooIProgressDialog.Title = "請稍後，取得 專案清單";
                var apiResultssss = await projectService.GetAsync();
                if (apiResultssss.Status == true)
                {
                    await projectService.WriteToFileAsync();
                }
                else
                {
                    await DialogHelper.ShowAPIResultIsFailureMessage(dialogService, apiResultssss);
                    return;
                }
                #endregion
                #region 取得人員清單
                fooIProgressDialog.Title = "請稍後，取得人員清單";
                apiResultssss = await myUserService.GetAsync();
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
                #region 取得 工作日誌表單
                fooIProgressDialog.Title = "請稍後，取得 工作日誌表單";
                apiResultssss = await workingLogService.GetAsync();
                if (apiResultssss.Status == true)
                {
                    await workingLogService.WriteToFileAsync();
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
