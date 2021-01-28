using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CommonLibrary.Helpers.Utilities
{
    public class UtilityHelper
    {
        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> CanConnectRemoteHostService()
        {
            #region 確認手機是否可以連上網路
            if (IsConnected() == false)
            {
                return false;
            }
            #endregion

            #region 確認是否遠端主機是否還活著
            var hostAvailable = await CrossConnectivity.Current.IsRemoteReachable(LOBGlobal.APIHost, LOBGlobal.APIPort, 5000);
            return hostAvailable;
            #endregion
        }
    }
}
