using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    public class LOBGlobal
    {
        #region 本機測試用 Web API
        // https://localhost:5001/
        public const string APIscheme = "https";
        public const string APIHost = "localhost";
        public const int APIPort = 5001;
        #endregion

        #region Azure 雲端主機 Web API
        //// https://hyperfullstack.azurewebsites.net/LeaveCategory
        //public const string APIscheme = "https";
        //public const string APIHost = "hyperfullstack.azurewebsites.net";
        //public const int APIPort = 443;
        #endregion
        public static string APIEndPointHost = $"{APIscheme}://{APIHost}:{APIPort}";
        public static string JSONDataKeyName = "JSONPayload";
        public static string AccessToken = "";
    }
}
