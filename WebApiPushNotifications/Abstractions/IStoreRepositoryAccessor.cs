using System;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Abstractions
{
    public interface IStoreRepositoryAccessor : IDisposable
    {
        ISqlServerStoreRepository StoreRepository { get; }
    }
}
