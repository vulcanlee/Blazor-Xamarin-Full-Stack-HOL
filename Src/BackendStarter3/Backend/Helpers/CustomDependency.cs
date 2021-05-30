﻿using Backend.ViewModels;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Backend.Helpers
{
    public static class CustomDependency
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            #region 註冊服務
            services.AddTransient<IChangePasswordService, ChangePasswordService>();
            services.AddTransient<IMenuRoleService, MenuRoleService>();
            services.AddTransient<IMenuDataService, MenuDataService>();
            services.AddTransient<IMyUserService, MyUserService>();
            services.AddTransient<DatabaseInitService>();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderMasterService, OrderMasterService>();
            services.AddTransient<IOrderItemService, OrderItemService>();

            #endregion

            #region 註冊 Razor Model
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<MenuRoleViewModel>();
            services.AddTransient<MenuDataViewModel>();
            services.AddTransient<MyUserViewModel>();

            services.AddTransient<OrderMasterViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<OrderItemViewModel>();
            #endregion

            #region 其他服務註冊
            services.AddTransient<TranscationResultHelper>();
            services.AddScoped<IEventAggregator, EventAggregator>();
            #endregion
            return services;
        }
    }
}
