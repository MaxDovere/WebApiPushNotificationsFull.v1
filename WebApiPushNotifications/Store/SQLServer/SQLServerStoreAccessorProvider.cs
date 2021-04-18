using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Store.SQLServer
{
    internal class SQLServerStoreAccessorProvider : IStoreRepositoryAccessorProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public SQLServerStoreAccessorProvider(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        public IStoreRepositoryAccessor GetStoreRepositoryAccessor()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return new SQLServerStoreAccessor(_serviceProvider.CreateScope());
            }
            else
            {
                return new SQLServerStoreAccessor(_httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISqlServerStoreRepository>());
            }
        }
    }
}
