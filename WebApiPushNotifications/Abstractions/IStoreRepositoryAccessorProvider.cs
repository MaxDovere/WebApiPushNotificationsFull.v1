namespace WebApiPushNotifications.Abstractions
{
    public interface IStoreRepositoryAccessorProvider
    {
        IStoreRepositoryAccessor GetStoreRepositoryAccessor();
    }
}
