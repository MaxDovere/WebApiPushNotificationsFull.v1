using System;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Abstractions
{
    //public interface IWebPushStoreAccessor : IDisposable
    //{
    //    IWebPushStore PushStore { get; }
    //}
    public interface IStoreRepositoryAccessor : IDisposable
    {
        ISqLServerStoreRepository PushStore { get; }
    }
}
