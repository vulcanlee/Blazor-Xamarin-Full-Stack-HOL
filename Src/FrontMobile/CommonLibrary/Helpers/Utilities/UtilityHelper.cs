using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
