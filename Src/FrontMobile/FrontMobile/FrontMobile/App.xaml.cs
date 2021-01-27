using Business.DataModel;
using Business.Helpers.ServiceHelps;
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
            containerRegistry.RegisterForNavigation<LeaveFormPage, LeaveFormPageViewModel>();
            containerRegistry.RegisterForNavigation<LeaveFormRecordPage, LeaveFormRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<OnCallPhonePage, OnCallPhonePageViewModel>();
            containerRegistry.RegisterForNavigation<WorkingLogPage, WorkingLogPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkingLogRecordPage, WorkingLogRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkingLogDetailPage, WorkingLogDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<WorkingLogDetailRecordPage, WorkingLogDetailRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<TravelExpensePage, TravelExpensePageViewModel>();
            containerRegistry.RegisterForNavigation<TravelExpenseDetailRecordPage, TravelExpenseDetailRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<TravelExpenseRecordPage, TravelExpenseRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<TravelExpenseDetailPage, TravelExpenseDetailPageViewModel>();
        }

        void RegisterOtherTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<TravelExpenseDetailService>();
            containerRegistry.Register<TravelExpenseService>();
            containerRegistry.Register<WorkingLogDetailService>();
            containerRegistry.Register<WorkingLogService>();
            containerRegistry.Register<LeaveFormService>();
            containerRegistry.Register<ProjectService>();
            containerRegistry.Register<OnCallPhoneService>();
            containerRegistry.Register<MyUserService>();
            containerRegistry.Register<LeaveCategoryService>();
            containerRegistry.Register<OnlyAdministratorService>();
            containerRegistry.Register<OnlyUserService>();
            containerRegistry.Register<SystemStatusService>();
            containerRegistry.Register<LoginService>();
            containerRegistry.Register<ExceptionRecordsService>();
            containerRegistry.Register<SystemEnvironmentsService>();
            containerRegistry.Register<AppExceptionsService>();
            containerRegistry.Register<RefreshTokenService>();
            containerRegistry.Register<RecordCacheHelper>();
            containerRegistry.RegisterSingleton<AppStatus>();

        }
    }
}
