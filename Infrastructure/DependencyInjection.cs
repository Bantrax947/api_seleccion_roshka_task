using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarInfrastructura(this IServiceCollection services)
        {
            services.AddTransient<ITaskRepository, TaskRepository>();
            return services;
        }
    }
}