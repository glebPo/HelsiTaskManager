using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HelsiTaskManager.WebAPI.Extensions
{
    public static class MongoDBConfigurationExtansion
    {
        public static IServiceCollection MongoDBConfiguration(this IServiceCollection services, string host, string dbName)
        {
            return services;
        }
    }
}
