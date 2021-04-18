using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.WebPushService
{
    internal class WebPushMessagesQueue : IWebPushMessagesQueue
    {
        private readonly ConcurrentQueue<WebPushNotificationMessages> _messages = new ConcurrentQueue<WebPushNotificationMessages>();
        private readonly SemaphoreSlim _messageEnqueuedSignal = new SemaphoreSlim(0);

        public async Task<WebPushNotificationMessages> Enqueue(WebPushNotificationMessages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _messages.Enqueue(message);

            _messageEnqueuedSignal.Release();

            return message;
        }

        public async Task<WebPushNotificationMessages> DequeueAsync(CancellationToken cancellationToken)
        {
            await _messageEnqueuedSignal.WaitAsync(cancellationToken);

            _messages.TryDequeue(out WebPushNotificationMessages message);

            return message;
        }
    }
}
