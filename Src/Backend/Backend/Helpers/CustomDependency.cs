﻿using Backend.RazorModels;
using Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class CustomDependency
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            #region 註冊服務
            services.AddTransient<IExceptionRecordService, ExceptionRecordService>();
            services.AddTransient<ITravelExpenseDetailService, TravelExpenseDetailService>();
            services.AddTransient<ITravelExpenseService, TravelExpenseService>();
            services.AddTransient<ILeaveFormService, LeaveFormService>();
            services.AddTransient<IWorkingLogDetailService, WorkingLogDetailService>();
            services.AddTransient<IWorkingLogService, WorkingLogService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IOnCallPhoneService, OnCallPhoneService>();
            services.AddTransient<ILeaveCategoryService, LeaveCategoryService>();
            services.AddTransient<IMyUserService, MyUserService>();
            services.AddTransient<DatabaseInitService>();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            #endregion

            #region 註冊 Razor Model
            services.AddTransient<ExceptionRecordRazorModel>();
            services.AddTransient<TravelExpenseDetailRazorModel>();
            services.AddTransient<TravelExpenseRazorModel>();
            services.AddTransient<LeaveFormRazorModel>();
            services.AddTransient<WorkingLogDetailRazorModel>();
            services.AddTransient<WorkingLogRazorModel>();
            services.AddTransient<ProjectRazorModel>();
            services.AddTransient<OnCallPhoneRazorModel>();
            services.AddTransient<LeaveCategoryRazorModel>();
            services.AddTransient<MyUserRazorModel>();

            services.AddTransient<OrderRazorModel>();
            services.AddTransient<ProductRazorModel>();
            services.AddTransient<OrderItemRazorModel>();
            #endregion

            #region 其他服務註冊
            #endregion
            return services;
        }
    }
}
