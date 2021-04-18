using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Repository;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Store.SQLServer
{
    public static class SSLServerServiceCollectionExtensions
    {
        private const string SQLSERVER_CONNECTION_STRING_NAME = "WebPushSqlServerDatabase";

        public static IServiceCollection AddSqlServerPushStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlServerDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(SQLSERVER_CONNECTION_STRING_NAME))
            );

            //services.AddTransient<IWebPushStore, SQLServerStore>();
            
            services.AddTransient<ISqlServerStoreRepository,SqLServerStoreRepository>();
            
            services.AddHttpContextAccessor();
            services.AddSingleton<IStoreRepositoryAccessorProvider, SQLServerStoreAccessorProvider>();

            return services;
        }
    }
}
