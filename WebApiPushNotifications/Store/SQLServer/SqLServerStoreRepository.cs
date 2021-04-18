using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiPushNotifications.Repository;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Store.SQLServer
{
    public partial class SubscriptionsStore : RepositoryGeneric<PushSubscriptionUser>, IRepositoryCommand<PushSubscriptionUser>
    {
        public SubscriptionsStore(DbContext context) : base(context) { }
    }
    public partial class NotificationsStore : RepositoryGeneric<PushNotificationUser>, IRepositoryCommand<PushNotificationUser>
    {
        public NotificationsStore(DbContext context) : base(context) { }
    }
    public partial class MessageTemplateStore : RepositoryGeneric<PushNotificationMessagesTemplate>, IRepositoryCommand<PushNotificationMessagesTemplate>
    {
        public MessageTemplateStore(DbContext context) : base(context) { }
    }
    public partial class NotificationTopicStore : RepositoryGeneric<PushSubscriptionTopic>, IRepositoryCommand<PushSubscriptionTopic>
    {
        public NotificationTopicStore(DbContext context) : base(context) { }
    }

    internal class SqLServerStoreRepository : ISqlServerStoreRepository
    {
        private readonly SqlServerDBContext _context;
        private readonly ILogger<SqLServerStoreRepository> _logger;
        private SubscriptionsStore _subscriptionsStore;
        private NotificationsStore _notificationsStore;
        private MessageTemplateStore _messageTemplateStore;
        private NotificationTopicStore _notificationTopicStore;

        public SqLServerStoreRepository(SqlServerDBContext context, ILogger<SqLServerStoreRepository> logger)
        {
            _logger = logger;
            _context = context;
            _subscriptionsStore = new SubscriptionsStore(_context);
            _notificationsStore = new NotificationsStore(_context);
            _messageTemplateStore = new MessageTemplateStore(_context);
            _notificationTopicStore = new NotificationTopicStore(_context);
        }

        public async Task InsertSubscriptionAsync(PushSubscriptionUser subscription) => await _subscriptionsStore.Insert(subscription);
        public async Task UpdateSubscriptionAsync(PushSubscriptionUser subscription) => await _subscriptionsStore.Update(subscription);
        public async Task RemoveSubscriptionAsync(PushSubscriptionUser subscription) => await _subscriptionsStore.Delete(subscription);
        public async Task<List<PushSubscriptionUser>> GetSubscriptionsAsync() => await _subscriptionsStore.Read();

        public async Task InsertNotificationAsync(PushNotificationUser notification) => await _notificationsStore.Insert(notification);
        public async Task UpdateNotificationAsync(PushNotificationUser notification) => await _notificationsStore.Update(notification);
        public async Task RemoveNotificationAsync(PushNotificationUser notification) => await _notificationsStore.Delete(notification);
        public async Task<List<PushNotificationUser>> GetNotificationsAsync() => await _notificationsStore.Read();

        public async Task InsertMessageTemplateAsync(PushNotificationMessagesTemplate message) => await _messageTemplateStore.Insert(message);
        public async Task UpdateMessageTemplateAsync(PushNotificationMessagesTemplate message) => await _messageTemplateStore.Update(message);
        public async Task RemoveMessageTemplateAsync(PushNotificationMessagesTemplate message) => await _messageTemplateStore.Delete(message);
        public async Task<List<PushNotificationMessagesTemplate>> GetMessagesTemplateAsync() => await _messageTemplateStore.Read();


        public async Task InsertSubscriptionTopicsAsync(PushSubscriptionTopic topic) => await _notificationTopicStore.Insert(topic);
        public async Task UpdateSubscriptionTopicAsync(PushSubscriptionTopic topic) => await _notificationTopicStore.Update(topic);
        public async Task RemoveSubscriptionTopicAsync(PushSubscriptionTopic topic) => await _notificationTopicStore.Delete(topic);
        public async Task<List<PushSubscriptionTopic>> GetSubscriptionTopicsAsync() => await _notificationTopicStore.Read();

        //funzioni sspeciali gestite direttamente dalla Api chiamata o qui ma sul context richiesto

        public async Task RemoveSubscriptionByEndpointAsync(string endpoint)
        {
            PushSubscriptionUser subscription = await _context.PushSubscriptionUser
                .Where(o => o.Endpoint == endpoint)
                .FirstOrDefaultAsync();
            
            await _subscriptionsStore.Delete(subscription);
        }

        public async Task<PushSubscriptionUser> GetSubscriptionByIdAsync(int subscriptionId)
        {
            return await _context.PushSubscriptionUser.AsNoTracking()
                .Where(o => o.SubscriptionId == subscriptionId)
                .SingleOrDefaultAsync();
        }
        public async Task<PushSubscriptionUser> GetSubscriptionByEndpointAsync(string endpoint)
        {
            return await this._context.PushSubscriptionUser.AsNoTracking()
                .Where(o => o.Endpoint == endpoint)
                .OrderByDescending(o => o.DateEndTime)
                .FirstOrDefaultAsync();
        }
        public async Task<List<PushNotificationUser>> GetNotificationsByTopicAsync(string topic)
        {
            return await _context.PushNotificationUser.AsNoTracking()
                .Where(o => o.Topic.Contains(topic))
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();
        }
        public async Task<PushSubscriptionTopic> GetSubscriptionTopicsByIdAsync(int topicId)
        {
            return await _context.PushSubscriptionTopic.AsNoTracking()
                .Where(o => o.TopicId == topicId)
                .SingleOrDefaultAsync();
        }

        public async Task<PushNotificationUser> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.PushNotificationUser.AsNoTracking()
                .Where(o => o.NotificationId == notificationId)
                .SingleOrDefaultAsync();
        }
        public async Task<PushNotificationMessagesTemplate> GetMessageTemplateByIdAsync(int messageid)
        {
            return await _context.PushNotificationMessagesTemplate.AsNoTracking()
                .Where(o => o.MessageId == messageid)
                .SingleOrDefaultAsync();
        }

        public Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action)
        {
            return ForEachSubscriptionAsync(action, CancellationToken.None);
        }
        public Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action, CancellationToken cancellationToken)
        {
            return _context.PushSubscriptionUser.AsNoTracking().ForEachAsync(action, cancellationToken);
        }
    }
}
