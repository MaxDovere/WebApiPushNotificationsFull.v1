using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Abstractions
{
    public interface IWebPushService
    {
        string PublicKey { get; }

        Task SendNotificationAsync(PushSubscriptionUser subscription, WebPushNotificationMessages message);

        Task SendNotificationAsync(PushSubscriptionUser subscription, WebPushNotificationMessages message, CancellationToken cancellationToken);

    }
}
