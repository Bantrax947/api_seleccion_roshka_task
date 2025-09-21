using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarCore(this IServiceCollection services)
        {
            services.AddTransient<ITaskService, TaskService>();   
            services.AddTransient<ISubTaskService, SubTaskService>();
            return services;
        }
    }
}