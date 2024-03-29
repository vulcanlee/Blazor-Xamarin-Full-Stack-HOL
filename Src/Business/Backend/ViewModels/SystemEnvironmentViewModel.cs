﻿using Backend.AdapterModels;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BAL.Helpers;
using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Interfaces;

namespace Backend.ViewModels
{
    public class SystemEnvironmentViewModel
    {
        #region 訊息說明之對話窗使用的變數
        /// <summary>
        /// 確認對話窗設定
        /// </summary>
        public ConfirmBoxModel ConfirmMessageBox { get; set; } = new ConfirmBoxModel();
        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public MessageBoxModel MessageBox { get; set; } = new MessageBoxModel();
        #endregion
        public SystemEnvironmentAdapterModel SystemEnvironmentModel { get; set; } = new SystemEnvironmentAdapterModel();
        public EditContext LocalEditContext { get; set; }
        public ISystemEnvironmentService SystemEnvironmentService { get; }
        public NavigationManager NavigationManager { get; }
        IRazorPage thisView;

        public SystemEnvironmentViewModel(ISystemEnvironmentService systemEnvironmentService,
            NavigationManager navigationManager)
        {
            SystemEnvironmentService = systemEnvironmentService;
            NavigationManager = navigationManager;
        }

        public void Setup(IRazorPage razorPage)
        {
            thisView = razorPage;
        }
        
        public void OnEditContestChanged(EditContext context)
        {
            LocalEditContext = context;
        }
        public string PasswordStrengthName { get; set; }
        public async Task OnSaveAsync()
        {
            #region 進行 Form Validation 檢查驗證作業
            if (LocalEditContext.Validate() == false)
            {
                return;
            }
            #endregion

            #region 其他資料完整性驗證
            #endregion

            #region 進行密碼變更
            await SystemEnvironmentService.UpdateAsync(SystemEnvironmentModel);
            MessageBox.Show("400px", "200px","通知","紀錄已經儲存成功", MessageBox.HiddenAsync);

            #endregion
        }
    }
}
