using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Store.SQLServer;
using WebApiPushNotifications.Configurations;
using WebApiPushNotifications.WebPushService;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPushServiceStore(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration.GetStoreType())
            {
                case StoreTypes.SQLServer:
                    services.AddSqlServerPushStore(configuration);
                    break;
                default:
                    throw new NotSupportedException($"[AddSqlServerPushStore]: Not supported {nameof(ISqlServerStoreRepository)} type.");
            }

            return services;
        }

        public static IServiceCollection AddWebPushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<WebPushNotificationConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("VapidKeysSettings").Bind(settings);
                });

            services.AddWebPushServicePushNotificationService(configuration);

            return services;
        }

        public static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            return services.AddScoped<INotificationService, NotificationService>();
        }

        public static IServiceCollection AddWebPushQueue(this IServiceCollection services)
        {
            services.AddSingleton<IWebPushMessagesQueue, WebPushMessagesQueue>();
            services.AddSingleton<IHostedService, WebPushMessagesDequeuer>();

            return services;
        }
    }
}
