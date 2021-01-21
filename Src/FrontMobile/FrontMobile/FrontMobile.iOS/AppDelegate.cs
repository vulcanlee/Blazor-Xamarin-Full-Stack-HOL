using Foundation;
using Prism;
using Prism.Ioc;
using System;
using UIKit;


namespace FrontMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            //{
            //    var foo = 1;
            //};
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            global::Xamarin.Forms.Forms.Init();

            #region 擴充套件初始化
            #endregion
            
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //ExceptionRecordsManager fooExceptionRecordsManager = new ExceptionRecordsManager();
            //await fooExceptionRecordsManager.ReadFromFileAsync();
            //ExceptionRecordResponseDTO fooObject = new ExceptionRecordResponseDTO()
            //{
            //    //CallStack = e.
            //};
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
