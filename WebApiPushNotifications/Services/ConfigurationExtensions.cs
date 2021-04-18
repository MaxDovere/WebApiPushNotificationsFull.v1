using System;
using Microsoft.Extensions.Configuration;
using WebApiPushNotifications.Abstractions;

namespace WebApiPushNotifications.Services
{
    internal static class ConfigurationExtensions
    {
        private const string STORE_TYPE_CONFIGURATION_KEY = "PushStoreType";
        private const string STORE_TYPE_SQLSERVER = "SqlServer";

        public static StoreTypes GetStoreType(this IConfiguration configuration)
        {
            string storeType = configuration.GetValue(STORE_TYPE_CONFIGURATION_KEY, STORE_TYPE_SQLSERVER);

            if (storeType.Equals(STORE_TYPE_SQLSERVER, StringComparison.InvariantCultureIgnoreCase))
            {
                return StoreTypes.SQLServer;
            }
            else
            {
                throw new NotSupportedException($"[GetStoreType]: Not supported {nameof(ConfigurationExtensions)} type.");
            }
        }
    }
}
