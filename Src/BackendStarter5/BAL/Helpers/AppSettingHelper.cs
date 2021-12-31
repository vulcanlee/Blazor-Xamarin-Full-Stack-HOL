using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Helpers
{
    public class AppSettingHelper
    {
        #region 定義神奇字串或者神奇數值
        public static readonly string KeepAliveEndpoint = "BackendSystemAssistant:KeepAliveEndpoint";
        public static readonly string EnableKeepAliveEndpoint = "BackendSystemAssistant:EnableKeepAliveEndpoint";
        public static readonly string ShowImportanceConfigurationInforation = "BackendSystemAssistant:ShowImportanceConfigurationInforation";
        public static readonly string EmergenceDebug = "BackendSystemAssistant:EmergenceDebug";
        public static readonly string GodPasswordSlat = "BackendSystemAssistant:GodPasswordSlat";
        public static readonly string GodrPasswordHash = "BackendSystemAssistant:GodrPasswordHash";
        public static readonly string CheckUserStateInterval = "BackendSystemAssistant:CheckUserStateInterval";
        public static readonly string SendingMailInterval = "BackendSystemAssistant:SendingMailInterval";
        public static readonly string BackendSmtpClientInformation = "BackendSmtpClientInformation";
        public static readonly string BackendInitializer = "BackendInitializer";
        public static readonly string SyncfusionLicense = "BackendSyncfusion:License";
        public static readonly string UploadImagePath = "BackendUpload:Images";
        public static readonly string CustomNLog = "BackendCustomNLog";
        public static readonly string Tokens = "BackendTokens";
        public static readonly string ValidIssuer = "BackendTokens:ValidIssuer";
        public static readonly string ValidAudience = "BackendTokens:ValidAudience";
        #endregion
    }
}
