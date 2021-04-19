using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Abstractions;
using WebApiPushNotifications.Configurations;
using WebApiPushNotifications.Formatters;
using WebApiPushNotifications.Shared;
using WebPush;

namespace WebApiPushNotifications.WebPushService
{

    internal class WebPushPushNotification : IWebPushService
    {
        private readonly WebPushNotificationConfig _options;
        private readonly WebPushClient _pushClient;
        private readonly IStoreRepositoryAccessorProvider _pushStoreAccessorProvider;

        private ILogger<WebPushPushNotification> _logger;

        public string PublicKey { get { return _options.PublicKey; } }

        public WebPushPushNotification(IOptions<WebPushNotificationConfig> options, IStoreRepositoryAccessorProvider pushStoreAccessorProvider, ILogger<WebPushPushNotification> logger)
        {
            _logger = logger;
            _options = options.Value;

            _pushClient = new WebPushClient();
            _pushStoreAccessorProvider = pushStoreAccessorProvider;

            _pushClient.SetVapidDetails(_options.Subject, _options.PublicKey, _options.PrivateKey);
        }

        public Task SendNotificationAsync(PushSubscriptionUser subscription, WebPushNotificationMessages message)
        {
            return SendNotificationAsync(subscription, message, CancellationToken.None);
        }

        public async Task SendNotificationAsync(PushSubscriptionUser subscription, WebPushNotificationMessages message, CancellationToken cancellationToken)
        {

            var vapidDetails = new VapidDetails(this._options.Subject, this._options.PublicKey, this._options.PrivateKey);
            PushSubscription pushSubscription = new WebPush.PushSubscription(subscription.Endpoint, subscription.P256DH, subscription.Auth);
            //var payload = System.Text.Json.JsonSerializer.Serialize(message);
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new JsonLowerCaseNamingStrategy() }; ;
            var payload = JsonConvert.SerializeObject(message, settings);

            try
            {
                await _pushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);                
            }
            catch (WebPushException ex)
            {
                {
                    using (IStoreRepositoryAccessor pushstoreAccessor = _pushStoreAccessorProvider.GetStoreRepositoryAccessor())
                    {
                        subscription.DateEndTime = DateTime.Now;
                        await pushstoreAccessor.StoreRepository.UpdateSubscriptionAsync(subscription);
                    }
                    _logger?.LogInformation("Subscription has expired or is no longer valid and has been removed.");
                }

                Console.Error.WriteLine("(WebPush) Error sending push notification: " + ex.Message);
            }
        }
    }
}