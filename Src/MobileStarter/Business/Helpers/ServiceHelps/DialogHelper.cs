using Business.DataModel;
using Business.Services;
using DataTransferObject.DTOs;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ServiceHelps
{
    public class DialogHelper
    {
        public static async Task<bool> ShowAPIResultIsFailureMessage(IPageDialogService dialogService, APIResult apiResult)
        {
            if (apiResult.Status == false)
            {
                await dialogService.DisplayAlertAsync("警告", apiResult.Message, "確定");
                return true;
            }
            return false;
        }
    }
}
