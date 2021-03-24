using System;
using Azure.Identity;
using BLOBi.Core.Models;
using BLOBi.Core.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLOBi.Core
{
    public static class BLOBiCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobICoreWithConnectionKey(this IServiceCollection services, string accountName, string accountKey, IConfiguration azureDefaults = default)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient($"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey};EndpointSuffix=core.windows.net");

                builder.SetAzureDefaults(azureDefaults);
            });

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICoreWithConnectionKey(this IServiceCollection services, IConfigurationSection azureStorageConfiguration, IConfiguration azureDefaults = default)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient($"DefaultEndpointsProtocol=https;AccountName={azureStorageConfiguration["AccountName"]};AccountKey={azureStorageConfiguration["AccountKey"]};EndpointSuffix=core.windows.net");

                builder.SetAzureDefaults(azureDefaults);
            });

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICoreWithConnectionString(this IServiceCollection services, string connectionString, IConfiguration azureDefaults = default)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(connectionString);

                builder.SetAzureDefaults(azureDefaults);
            });

            services.Configure<AzureStorageManagement>(options =>
            {
                options.ConnectionString = connectionString;
            });

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICoreWithConnectionString(this IServiceCollection services, IConfigurationSection azureStorageConfiguration, IConfiguration azureDefaults = default)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(azureStorageConfiguration["ConnectionString"]);

                builder.SetAzureDefaults(azureDefaults);
            });

            services.Configure<AzureStorageManagement>(options =>
            {
                options.ConnectionString = azureStorageConfiguration["ConnectionString"];
            });

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICoreWithManagedIdentity(this IServiceCollection services, Uri serviceUri, ChainedTokenCredential credentials, bool debugEmulatorOverride = false, IConfiguration azureDefaults = default)
        {
            if (debugEmulatorOverride)
            {
                services.AddAzureClients(builder =>
                {
#if DEBUG
                    builder.AddBlobServiceClient(connectionString: "UseDevelopmentStorage = true");
#else
                    builder.AddBlobServiceClient(serviceUri: serviceUri);
#endif

                    builder.UseCredential(credentials);

                    builder.SetAzureDefaults(azureDefaults);
                });
            }
            else
            {
                services.AddAzureClients(builder =>
                {
                    builder.AddBlobServiceClient(serviceUri: serviceUri);

                    builder.UseCredential(credentials);

                    builder.SetAzureDefaults(azureDefaults);
                });
            }

            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddBlobICoreWithManagedIdentity(this IServiceCollection services, IConfigurationSection azureStorageConfiguration, ChainedTokenCredential credentials, bool debugEmulatorOverride = false, IConfiguration azureDefaults = default)
        {
            if (debugEmulatorOverride)
            {
                services.AddAzureClients(builder =>
                {
#if DEBUG
                    builder.AddBlobServiceClient(connectionString: "UseDevelopmentStorage = true");
#else
                        builder.AddBlobServiceClient(new Uri(azureStorageConfiguration["ServiceUri"]));
#endif

                    builder.UseCredential(credentials);

                    builder.SetAzureDefaults(azureDefaults);
                });
            }
            else
            {
                services.AddAzureClients(builder =>
                {
                    builder.AddBlobServiceClient(new Uri(azureStorageConfiguration["ServiceUri"]));

                    builder.UseCredential(credentials);

                    builder.SetAzureDefaults(azureDefaults);
                });
            }

            RegisterServices(services);

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBlobContainerService, BlobContainerService>();
            services.AddScoped<IBlobMetaDataService, BlobMetaDataService>();
            services.AddScoped<IBlobService, BlobService>();
        }

        private static AzureClientFactoryBuilder SetAzureDefaults(this AzureClientFactoryBuilder builder, IConfiguration azureDefaults)
        {
            if (azureDefaults != null)
            {
                builder.ConfigureDefaults(azureDefaults);
            }

            return builder;
        }
    }
}
