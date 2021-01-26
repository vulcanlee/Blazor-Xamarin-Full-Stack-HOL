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
    public class WorkingLogDetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly WorkingLogDetailService workingLogDetailService;
        private readonly ProjectService projectService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;

        public ObservableCollection<ProjectDto> PickerDataItems { get; set; } = new ObservableCollection<ProjectDto>();
        public ObservableCollection<WorkingLogDetailDto> DataItems { get; set; } = new ObservableCollection<WorkingLogDetailDto>();
        public bool IsRefresh { get; set; }
        public WorkingLogDetailDto SelectedItem { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public WorkingLogDto MasterItem { get; set; }


        public WorkingLogDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
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

            #region 新增紀錄
            AddCommand = new DelegateCommand(async () =>
            {
                NavigationParameters paras = new NavigationParameters();
                var fooObject = new WorkingLogDetailDto();
                fooObject.WorkingLogId = MasterItem.Id;
                paras.Add(MagicStringHelper.CurrentSelectdItemParameterName, fooObject);
                paras.Add(MagicStringHelper.CrudActionName, MagicStringHelper.CrudAddAction);
                paras.Add(MagicStringHelper.MasterRecordActionName, MasterItem);
                await navigationService.NavigateAsync("WorkingLogDetailRecordPage", paras);
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
                await navigationService.NavigateAsync("WorkingLogDetailRecordPage", paras);
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
                MasterItem = parameters.GetValue<WorkingLogDto>(MagicStringHelper.MasterRecordActionName);
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
            await workingLogDetailService.ReadFromFileAsync();
            DataItems.Clear();
            foreach (var item in workingLogDetailService.Items)
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
                apiResultssss = await workingLogDetailService.GetAsync(MasterItem.Id);
                if (apiResultssss.Status == true)
                {
                    await workingLogDetailService.WriteToFileAsync();
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
