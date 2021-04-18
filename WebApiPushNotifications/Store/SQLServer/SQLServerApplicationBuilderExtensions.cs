using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiPushNotifications.Store.SQLServer
{
    public static class SQLServerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSQLServerPushStore(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SqlServerDBContext context = serviceScope.ServiceProvider.GetService<SqlServerDBContext>();
                context.Database.EnsureCreated();
            }

            return app;
        }
    }
}
