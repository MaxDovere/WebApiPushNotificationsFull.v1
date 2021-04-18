using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Repository;
using WebApiPushNotifications.Store.SQLServer;

namespace WebApiPushNotifications.Services
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePushStore(this IApplicationBuilder app)
        {
            StoreTypes storeType = ((IConfiguration)app.ApplicationServices.GetService(typeof(IConfiguration))).GetStoreType();

            switch (storeType)
            {
                case StoreTypes.SQLServer:
                    app.UseSQLServerPushStore();
                    break;
                default:
                    throw new NotSupportedException($"[UsePushStore]: Not supported {nameof(ISqlServerStoreRepository)} type.");
            }

            return app;
        }
    }
}
