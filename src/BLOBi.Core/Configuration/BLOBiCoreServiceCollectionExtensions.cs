using BLOBi.Core.Models;
using BLOBi.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLOBi.Core
{
    public static class BLOBiCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobICore(this IServiceCollection services, IConfigurationSection blobiConfiguration)
        {
            services.Configure<AzureStorageManagement>(blobiConfiguration);

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICore(this IServiceCollection services, BlobiConfigurationOptions blobiOptions)
        {
            services.Configure<AzureStorageManagement>(options =>
            {
                options.AccountKey = blobiOptions.AccountKey;
                options.AccountName = blobiOptions.AccountName;
                options.ConnectionString = blobiOptions.ConnectionString;
            });

            RegisterServices(services);

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IBlobContainerService, BlobContainerService>();
        }
    }
}
