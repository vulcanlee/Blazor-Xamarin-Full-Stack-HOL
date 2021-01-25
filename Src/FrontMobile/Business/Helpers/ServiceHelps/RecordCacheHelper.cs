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

namespace Business.Helpers.ServiceHelps
{
    public class RecordCacheHelper
    {
        private readonly IPageDialogService dialogService;
        private readonly SystemEnvironmentsService systemEnvironmentsService;
        private readonly SystemStatusService systemStatusService;
        private readonly AppStatus appStatus;
        private readonly RefreshTokenService refreshTokenService;
        private readonly ExceptionRecordsService exceptionRecordsService;

        public RecordCacheHelper(IPageDialogService dialogService,
            SystemEnvironmentsService systemEnvironmentsService,
            SystemStatusService systemStatusService, AppStatus appStatus, RefreshTokenService refreshTokenService,
            ExceptionRecordsService exceptionRecordsService)
        {
            this.dialogService = dialogService;
            this.systemEnvironmentsService = systemEnvironmentsService;
            this.systemStatusService = systemStatusService;
            this.appStatus = appStatus;
            this.refreshTokenService = refreshTokenService;
            this.exceptionRecordsService = exceptionRecordsService;
        }

        public async Task<bool> RefreshAsync(IProgressDialog progressDialog)
        {
            APIResult fooAPIResult;
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper
                .CheckAndRefreshToken(dialogService, refreshTokenService,
                systemStatusService, appStatus);
            if (fooRefreshTokenResult == false)
            {
                return false;
            }
            //progressDialog.Title = $"回報例外異常資料中";
            //await exceptionRecordsService.ReadFromFileAsync();
            //if (exceptionRecordsService.Items.Count > 0)
            //{
            //    List<ExceptionRecordRequestDTO> fooExceptionRecordRequestDTOList = new List<ExceptionRecordRequestDTO>();
            //    foreach (var item in exceptionRecordsService.Items)
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
            //    fooAPIResult = await exceptionRecordsService.PostAsync(fooExceptionRecordRequestDTOList);
            //    if (fooAPIResult.Status != true)
            //    {
            //        await dialogService.DisplayAlertAsync("回報例外異常資料中 發生錯誤", fooAPIResult.Message, "確定");
            //        return false;
            //    }
            //}
            //progressDialog.Title = $"更新系統最新狀態資料中";
            //fooAPIResult = await systemEnvironmentsService.GetAsync();
            //if (fooAPIResult.Status != true)
            //{
            //    await dialogService.DisplayAlertAsync("更新系統最新狀態資料中 發生錯誤", fooAPIResult.Message, "確定");
            //    return false;
            //}

            return true;
        }
    }
}
