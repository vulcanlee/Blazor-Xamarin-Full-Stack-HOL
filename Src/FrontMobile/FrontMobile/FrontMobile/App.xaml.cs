using Business.DataModel;
using Business.Helpers.ManagerHelps;
using Business.Services;
using FrontMobile.ViewModels;
using FrontMobile.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

[assembly: ExportFont("materialdesignicons-webfont.ttf", Alias = "MaterialDesignIcons")]
namespace FrontMobile
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("SplashPage");
            //await NavigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
            containerRegistry.RegisterForNavigation<MDPage, MDPageViewModel>();
            containerRegistry.RegisterForNavigation<NaviPage, NaviPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();

            RegisterOtherTypes(containerRegistry);
        }

        void RegisterOtherTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<OnlyAdministratorManager>();
            containerRegistry.Register<OnlyUserManager>();
            containerRegistry.Register<SystemStatusManager>();
            containerRegistry.Register<LoginManager>();
            containerRegistry.Register<ExceptionRecordsManager>();
            containerRegistry.Register<SystemEnvironmentsManager>();
            containerRegistry.Register<AppExceptionsManager>();
            containerRegistry.Register<RefreshTokenManager>();
            containerRegistry.Register<RecordCacheHelper>();
            containerRegistry.RegisterSingleton<AppStatus>();

        }
    }
}
