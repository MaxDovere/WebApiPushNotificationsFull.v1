using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Repository
{
    public interface ISqlServerStoreRepository
    {
        Task<List<PushSubscriptionUser>> GetSubscriptionsAsync();
        Task<List<PushNotificationUser>> GetNotificationsAsync();
        Task<List<PushNotificationMessagesTemplate>> GetMessagesTemplateAsync();
        Task<List<PushSubscriptionTopic>> GetSubscriptionTopicsAsync();

        Task InsertSubscriptionAsync(PushSubscriptionUser subscription);
        Task InsertNotificationAsync(PushNotificationUser notification);
        Task InsertMessageTemplateAsync(PushNotificationMessagesTemplate message);
        Task InsertSubscriptionTopicsAsync(PushSubscriptionTopic topic);

        Task UpdateSubscriptionAsync(PushSubscriptionUser subscription);
        Task UpdateNotificationAsync(PushNotificationUser notification);
        Task UpdateMessageTemplateAsync(PushNotificationMessagesTemplate message);
        Task UpdateSubscriptionTopicAsync(PushSubscriptionTopic topic);

        Task RemoveSubscriptionAsync(PushSubscriptionUser subscription);
        Task RemoveNotificationAsync(PushNotificationUser notification);
        Task RemoveMessageTemplateAsync(PushNotificationMessagesTemplate message);
        Task RemoveSubscriptionTopicAsync(PushSubscriptionTopic topic);
        
        //funzioni speciali gestite direttamente dalle api chiamate
        Task RemoveSubscriptionByEndpointAsync(string endpoint);

        Task<PushSubscriptionUser> GetSubscriptionByIdAsync(int subscriptionId);
        Task<PushSubscriptionUser> GetSubscriptionByEndpointAsync(string endpoint);
        Task<PushNotificationUser> GetNotificationByIdAsync(int notificationId);
        Task<List<PushNotificationUser>> GetNotificationsByTopicAsync(string topic);
        Task<PushNotificationMessagesTemplate> GetMessageTemplateByIdAsync(int messageid);
        Task<PushSubscriptionTopic> GetSubscriptionTopicsByIdAsync(int topicId);
        Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action);
        Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action, CancellationToken cancellationToken);

    }
}