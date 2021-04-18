using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiPushNotifications.Abstractions;

namespace WebApiPushNotifications.WebPushService
{
    public static class WebPushPushNotificationCollectionExtensions
    {
        public static IServiceCollection AddWebPushServicePushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            
            //services.AddMemoryVapidTokenCache();

            services.AddTransient<IWebPushService, WebPushPushNotification>();

            return services;
        }
    }
}
