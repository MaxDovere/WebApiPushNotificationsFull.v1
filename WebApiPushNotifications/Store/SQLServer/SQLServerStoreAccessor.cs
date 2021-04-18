using Microsoft.Extensions.DependencyInjection;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Store.SQLServer
{
    internal class SQLServerStoreAccessor : IStoreRepositoryAccessor
    {
        private IServiceScope _serviceScope;

        public ISqlServerStoreRepository StoreRepository { get; private set; }

        public SQLServerStoreAccessor(ISqlServerStoreRepository pushstore)
        {
            StoreRepository = pushstore;
        }

        public SQLServerStoreAccessor(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
            StoreRepository = _serviceScope.ServiceProvider.GetService<ISqlServerStoreRepository>();
        }

        public void Dispose()
        {
            StoreRepository = null;

            if (!(_serviceScope is null))
            {
                _serviceScope.Dispose();
                _serviceScope = null;
            }
        }
    }
}
