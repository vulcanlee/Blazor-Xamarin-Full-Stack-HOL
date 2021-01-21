using Acr.UserDialogs;
using Business.DataModel;
using Business.Services;
using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ManagerHelps
{
    public class RecordCacheHelper
    {
        private readonly IPageDialogService dialogService;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly ExceptionRecordsManager exceptionRecordsManager;

        public RecordCacheHelper(IPageDialogService dialogService,
            SystemEnvironmentsManager systemEnvironmentsManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus, RefreshTokenManager refreshTokenManager,
            ExceptionRecordsManager exceptionRecordsManager)
        {
            this.dialogService = dialogService;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.refreshTokenManager = refreshTokenManager;
            this.exceptionRecordsManager = exceptionRecordsManager;
        }

        public async Task<bool> RefreshAsync(IProgressDialog progressDialog)
        {
            APIResult fooAPIResult;
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
            if (fooRefreshTokenResult == false)
            {
                return false;
            }
            //progressDialog.Title = $"回報例外異常資料中";
            //await exceptionRecordsManager.ReadFromFileAsync();
            //if (exceptionRecordsManager.Items.Count > 0)
            //{
            //    List<ExceptionRecordRequestDTO> fooExceptionRecordRequestDTOList = new List<ExceptionRecordRequestDTO>();
            //    foreach (var item in exceptionRecordsManager.Items)
            //    {
            //        ExceptionRecordRequestDTO fooExceptionRecordRequestDTO = new ExceptionRecordRequestDTO()
            //        {
            //            CallStack = item.CallStack,
            //            DeviceModel = item.DeviceModel,
            //            DeviceName = item.DeviceName,
            //            ExceptionTime = item.ExceptionTime,
            //            Message = item.Message,
            //            OSType = item.OSType,
            //            OSVersion = item.OSVersion,
            //            User = new UserDTO() { Id = appStatus.SystemStatus.UserID },
            //        };
            //        fooExceptionRecordRequestDTOList.Add(fooExceptionRecordRequestDTO);
            //    }
            //    fooAPIResult = await exceptionRecordsManager.PostAsync(fooExceptionRecordRequestDTOList);
            //    if (fooAPIResult.Status != true)
            //    {
            //        await dialogService.DisplayAlertAsync("回報例外異常資料中 發生錯誤", fooAPIResult.Message, "確定");
            //        return false;
            //    }
            //}
            //progressDialog.Title = $"更新系統最新狀態資料中";
            //fooAPIResult = await systemEnvironmentsManager.GetAsync();
            //if (fooAPIResult.Status != true)
            //{
            //    await dialogService.DisplayAlertAsync("更新系統最新狀態資料中 發生錯誤", fooAPIResult.Message, "確定");
            //    return false;
            //}

            return true;
        }
    }
}
