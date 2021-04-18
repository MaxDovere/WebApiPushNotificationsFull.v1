using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Abstractions
{
    public interface IWebPushStore
    {

        Task InsertSubscriptionsAsync(PushSubscriptionUser subscription);

        Task RemoveSubscriptionAsync(string endpoint);

        Task<List<PushSubscriptionUser>> GetSubscriptionAsync();

        Task<PushSubscriptionUser> GetSubscriptionByEndpointAsync(string endpoint);

        Task<PushSubscriptionUser> GetSubscriptionByIdAsync(int subscriptionId);

        Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action);

        Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action, CancellationToken cancellationToken);

        Task InsertNotificationAsync(PushNotificationUser notification);
                
        Task RemoveNotificationAsync(int notificationId);

        Task<List<PushNotificationUser>> GetNotificationsAsync();

        Task<List<PushNotificationUser>> GetNotificationsByTopicAsync(string topic);

        Task<PushNotificationUser> GetNotificationByIdAsync(int notificationId);

        Task<List<PushNotificationMessagesTemplate>> GetMessagesTemplateAsync();
        
        Task<PushNotificationMessagesTemplate> GetMessageTemplateByIdAsync(int messageid);

        Task<List<PushSubscriptionTopic>> GetTopicsAsync();

        Task InsertTopicAsync(PushSubscriptionTopic topic);

        Task InsertMessageTemplateAsync(PushNotificationMessagesTemplate message);

        Task RemoveMessageTemplateAsync(int messagid);

        Task RemoveTopicAsync(int topicId);
    }
}
