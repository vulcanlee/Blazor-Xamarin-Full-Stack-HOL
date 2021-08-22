using Backend.ViewModels;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using Backend.Events;
using Backend.Models;

namespace Backend.Helpers
{
    public static class CustomDependency
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            #region 註冊服務
            services.AddTransient<IAuditMasterService, AuditMasterService>();
            services.AddTransient<IPhaseMessageService, PhaseMessageService>();
            services.AddTransient<IPhaseCategoryService, PhaseCategoryService>();
            services.AddTransient<IPolicyDetailService, PolicyDetailService>();
            services.AddTransient<IPolicyHeaderService, PolicyHeaderService>();

            services.AddTransient<ISystemLogService, SystemLogService>();
            services.AddTransient<IChangePasswordService, ChangePasswordService>();
            services.AddTransient<IMenuRoleService, MenuRoleService>();
            services.AddTransient<IMenuDataService, MenuDataService>();
            services.AddTransient<IMyUserService, MyUserService>();
            services.AddTransient<DatabaseInitService>();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderMasterService, OrderMasterService>();
            services.AddTransient<IOrderItemService, OrderItemService>();

            #endregion

            #region 註冊 ViewModel
            services.AddTransient<AuditMasterViewModel>();
            services.AddTransient<PhaseMessageViewModel>();
            services.AddTransient<PhaseCategoryViewModel>();
            services.AddTransient<PolicyDetailViewModel>();
            services.AddTransient<PolicyHeaderViewModel>();

            services.AddTransient<SystemLogViewModel>();
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<MenuRoleViewModel>();
            services.AddTransient<MenuDataViewModel>();
            services.AddTransient<MyUserViewModel>();

            services.AddTransient<OrderMasterViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<OrderItemViewModel>();
            #endregion

            #region 其他服務註冊
            services.AddSingleton<SystemBroadcast>();
            services.AddTransient<ImportDataHelper>();
            services.AddTransient<TranscationResultHelper>();
            services.AddTransient<SystemLogHelper>();
            services.AddScoped<BlazorAppContext>();
            services.AddScoped<IEventAggregator, EventAggregator>();
            #endregion
            return services;
        }
    }
}
