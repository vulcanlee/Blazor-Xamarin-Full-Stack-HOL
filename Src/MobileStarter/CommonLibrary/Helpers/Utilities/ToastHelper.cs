using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Helpers.Utilities
{
    public class ToastHelper
    {
        public static void ShowToast(string message, int howLong=2, ToastPosition location = ToastPosition.Bottom)
        {
            IUserDialogs dialogs = UserDialogs.Instance;
            ToastConfig.DefaultBackgroundColor = System.Drawing.Color.Black;
            ToastConfig.DefaultMessageTextColor = System.Drawing.Color.White;
            ToastConfig.DefaultActionTextColor = System.Drawing.Color.Yellow;

            dialogs.Toast(new ToastConfig($"{message}")
                .SetDuration(TimeSpan.FromSeconds(howLong))
                .SetPosition(location)
                );

        }
    }
}
