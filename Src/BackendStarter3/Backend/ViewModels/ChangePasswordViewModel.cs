using Backend.AdapterModels;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ShareBusiness.Helpers;
using ShareDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ViewModels
{
    public class ChangePasswordViewModel
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
        public ChangePasswordModel ChangePasswordModel { get; set; } = new ChangePasswordModel();
        public EditContext LocalEditContext { get; set; }
        public IChangePasswordService ChangePasswordService { get; }
        public NavigationManager NavigationManager { get; }

        public ChangePasswordViewModel(IChangePasswordService changePasswordService,
            NavigationManager navigationManager)
        {
            ChangePasswordService = changePasswordService;
            NavigationManager = navigationManager;
        }
        public void OnEditContestChanged(EditContext context)
        {
            LocalEditContext = context;
        }
        public async Task OnSaveAsync()
        {
            MyUserAdapterModel myUserAdapterModel = new MyUserAdapterModel();
            #region 進行 Form Validation 檢查驗證作業
            if (LocalEditContext.Validate() == false)
            {
                return;
            }
            #endregion

            #region 其他資料完整性驗證
            if (ChangePasswordModel.NewPasswordAgain != ChangePasswordModel.NewPassword)
            {
                MessageBox.Show("400px", "200px",
                    ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.警告),
                    ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.新密碼2次輸入須相同));
                return;
            }
            else
            {
                myUserAdapterModel = await ChangePasswordService.GetCurrentUser();
                if (myUserAdapterModel == null)
                {
                    MessageBox.Show("400px", "200px",
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.警告),
                        ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.使用者不存在));
                    return;
                }
            }
            #endregion

            #region 進行密碼變更
            await ChangePasswordService.ChangePassword(myUserAdapterModel, ChangePasswordModel.NewPassword);
            MessageBox.Show("400px", "200px",
                ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.警告),
                ErrorMessageMappingHelper.Instance.GetErrorMessage(ErrorMessageEnum.密碼已經變更成功));

            #endregion
        }

        public async Task CloseMessageBox()
        {
            MessageBox.Hidden();
            await Task.Yield();
            NavigationManager.NavigateTo("/Logout", true);
        }
    }
}
