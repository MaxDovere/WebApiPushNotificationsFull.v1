using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Abstractions
{
    public interface IWebPushMessagesQueue
    {
        Task<WebPushNotificationMessages> Enqueue(WebPushNotificationMessages message);

        Task<WebPushNotificationMessages> DequeueAsync(CancellationToken cancellationToken);
    }
}
