using Business.DataModel;
using Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.ServiceHelps
{
    public class AppStatusHelper
    {
        public static async Task<bool> ReadAndUpdateAppStatus(SystemStatusService systemStatusService, AppStatus appStatus)
        {
            await systemStatusService.ReadFromFileAsync();
            appStatus.SystemStatus = systemStatusService.SingleItem;
            return true;
        }
        public static async Task<bool> WriteAndUpdateAppStatus(SystemStatusService systemStatusService, AppStatus appStatus)
        {
            await systemStatusService.WriteToFileAsync();
            appStatus.SystemStatus = systemStatusService.SingleItem;
            return true;
        }
    }
}
