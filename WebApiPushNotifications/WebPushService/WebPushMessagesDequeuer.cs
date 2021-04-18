using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.WebPushService
{
    internal class WebPushMessagesDequeuer : IHostedService
    {
        private readonly IStoreRepositoryAccessorProvider _pushStoreAccessorProvider;
        private readonly IWebPushMessagesQueue _messagesQueue;
        private readonly IWebPushService _pushService;
        private readonly CancellationTokenSource _stopTokenSource = new CancellationTokenSource();

        private Task _dequeueMessagesTask;

        public WebPushMessagesDequeuer(IWebPushMessagesQueue messagesQueue, IStoreRepositoryAccessorProvider pushStoreAccessorProvider, IWebPushService pushService)
        {
            _pushStoreAccessorProvider = pushStoreAccessorProvider;
            _messagesQueue = messagesQueue;
            _pushService = pushService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _dequeueMessagesTask = Task.Run(DequeueMessagesAsync);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _stopTokenSource.Cancel();

            return Task.WhenAny(_dequeueMessagesTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        private async Task DequeueMessagesAsync()
        {
            while (!_stopTokenSource.IsCancellationRequested)
            {
                WebPushNotificationMessages message = await _messagesQueue.DequeueAsync(_stopTokenSource.Token);

                if (!_stopTokenSource.IsCancellationRequested)
                {
                    using (IStoreRepositoryAccessor pushStoreAccessor = _pushStoreAccessorProvider.GetStoreRepositoryAccessor())
                    {
                        await pushStoreAccessor.StoreRepository.ForEachSubscriptionAsync((PushSubscriptionUser subscription) =>
                        {
                            // Fire-and-forget 
                            _pushService.SendNotificationAsync(subscription, message, _stopTokenSource.Token);
                        }, 
                            _stopTokenSource.Token);
                    }

                }
            }

        }
    }
}
