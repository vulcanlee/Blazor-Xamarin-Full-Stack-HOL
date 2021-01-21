using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Business.DataModel;
using Business.Services;
using DataTransferObject.DTOs;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Threading.Tasks;
using Unity;
using Xamarin.Essentials;

#region 宣告需要用到的權限 Permissions
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
#endregion
namespace FrontMobile.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            #region 擴充套件初始化
            UserDialogs.Init(this);
            #endregion

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            IContainerProvider myContainer = (App.Current as PrismApplication).Container;
            AppExceptionsManager appExceptionsManager = myContainer
                .Resolve<AppExceptionsManager>();
            AppStatus appStatus = myContainer
                .Resolve<AppStatus>();
            Task.Run(async () =>
            {
                await appExceptionsManager.ReadFromFileAsync();
                ExceptionRecordDto fooObject = new ExceptionRecordDto()
                {
                    CallStack = (e.ExceptionObject as Exception).StackTrace,
                    ExceptionTime = DateTime.Now,
                    Message = (e.ExceptionObject as Exception).Message,
                    User = new UserDTO() { Id = appStatus.SystemStatus.UserID },
                    DeviceModel = DeviceInfo.Model,
                    DeviceName = DeviceInfo.Name,
                    OSType = DeviceInfo.Platform.ToString(),
                    OSVersion = DeviceInfo.Version.ToString(),
                };
                appExceptionsManager.Items.Add(fooObject);
                await appExceptionsManager.WriteToFileAsync();
            }).Wait();
            
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

