using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontMobile.ViewModels
{
    using System.ComponentModel;
    using Business.DataModel;
    using Business.Services;
    using CommonLibrary.Helpers.Magics;
    using DataTransferObject.DTOs;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class LeaveFormDetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LeaveFormService leaveFormService;
        private readonly LeaveCategoryService leaveCategoryService;
        private readonly MyUserService myUserService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        public LeaveFormDto SelectedItem { get; set; }

        public LeaveFormDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LeaveFormService leaveFormService, LeaveCategoryService leaveCategoryService,
            MyUserService myUserService,
            RefreshTokenService refreshTokenService,
            SystemStatusService systemStatusService, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.leaveFormService = leaveFormService;
            this.leaveCategoryService = leaveCategoryService;
            this.myUserService = myUserService;
            this.refreshTokenService = refreshTokenService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var fooObject = parameters.GetValue<LeaveFormDto>(MagicStringHelper.CurrentSelectdItemParameterName);
            fooObject.SetDate();
            SelectedItem = fooObject;
        }

    }
}
