using Blogger.Services;
using Blogger.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Helpers
{
    public static class CustomDependencyHelper
    {
        public static void AddCustomDependency(this IServiceCollection services)
        {
            services.AddScoped<BlogViewModel>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IMyUserAuthenticationService, MyUserAuthenticationService>();
            services.AddScoped<IMyUserAuthenticationService, MyUserAuthenticationService>();
            services.AddScoped<IMyUserService, MyUserService>();
        }
    }
}
