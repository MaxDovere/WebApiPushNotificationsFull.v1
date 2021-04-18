using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiPushNotifications.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Store.SQLServer
{
    internal class SQLServerStore : IWebPushStore
    {
        private readonly SqlServerDBContext _context;
        private readonly ILogger<SQLServerStore> _logger;

        public SQLServerStore(SqlServerDBContext context, ILogger<SQLServerStore> logger)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// Inserimento sottiscrizione nel database
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public async Task InsertSubscriptionsAsync(PushSubscriptionUser subscription)
        {
            _context.PushSubscriptionUser.Add(subscription);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Eliminazione della sottoscrizione dal database
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task RemoveSubscriptionAsync(string endpoint)
        {
            PushSubscriptionUser subscription = await _context.PushSubscriptionUser
                .Where(o => o.Endpoint == endpoint)
                .FirstOrDefaultAsync();

            _context.PushSubscriptionUser.Remove(subscription);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Restuisce tutte le sottoscrizioni dal database
        /// </summary>
        /// <returns></returns>
        public async Task<List<PushSubscriptionUser>> GetSubscriptionAsync()
        {
            return await _context.PushSubscriptionUser.AsNoTracking()
                .OrderByDescending(o => o.DateEndTime)
                .ToListAsync();
        }
        /// <summary>
        /// Restituisce una sottoscrizione in base all'endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<PushSubscriptionUser> GetSubscriptionByEndpointAsync(string endpoint)
        {
            return await _context.PushSubscriptionUser.AsNoTracking()
                .Where(o => o.Endpoint == endpoint)
                .OrderByDescending(o => o.DateEndTime)
                .FirstOrDefaultAsync();
        }
        ///// <summary>
        ///// Restituisce una sottoscrizione inn base all'id salvata sul database
        ///// </summary>
        ///// <param name="subscriptionId"></param>
        ///// <returns></returns>
        public async Task<PushSubscriptionUser> GetSubscriptionByIdAsync(int subscriptionId)
        {
            return await _context.PushSubscriptionUser.AsNoTracking()
                .Where(o => o.SubscriptionId == subscriptionId)
                .SingleOrDefaultAsync();

        }
        /// <summary>
        /// esegue una azione su tutti i record che sono stati recuperati dal database
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action)
        {
            return ForEachSubscriptionAsync(action, CancellationToken.None);
        }
        public Task ForEachSubscriptionAsync(Action<PushSubscriptionUser> action, CancellationToken cancellationToken)
        {
            return _context.PushSubscriptionUser.AsNoTracking().ForEachAsync(action, cancellationToken);
        }
        /// <summary>
        /// Inserisce una notifica sul database
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public async Task InsertNotificationAsync(PushNotificationUser notification)
        {
            _context.PushNotificationUser.Add(notification);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Elimina una notifica dal database
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task RemoveNotificationAsync(int notificationId)
        {
            PushNotificationUser notification = await _context.PushNotificationUser.AsNoTracking()
                .Where(o => o.NotificationId == notificationId)
                .FirstOrDefaultAsync();

            _context.PushNotificationUser.Remove(notification);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Restitisce tutte le notifiche dal database
        /// </summary>
        /// <returns></returns>
        public async Task<List<PushNotificationUser>> GetNotificationsAsync()
        {
            return await _context.PushNotificationUser.AsNoTracking()
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            // return notifications; //.Select(o => PushNotificationWithStatus.FromExecute(o)).ToList();
        }
        /// <summary>
        /// Restituisce tutte le notifiche che risultano dalla ricerca per Topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task<List<PushNotificationUser>> GetNotificationsByTopicAsync(string topic)
        {
            return await _context.PushNotificationUser.AsNoTracking()
                .Where(o => o.Topic.Contains(topic))
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            // return notifications; //.Select(o => PushNotificationWithStatus.FromExecute(o)).ToList();
        }
        /// <summary>
        /// Restituisce una sola notificain base all'Id salvato nel database
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task<PushNotificationUser> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.PushNotificationUser.AsNoTracking()
                .Where(o => o.NotificationId == notificationId)
                .SingleOrDefaultAsync();

            // return notification; PushNotificationWithStatus.FromExecute(notification);
        }
        /// <summary>
        /// Restituisce tutti i messaggi Template dal database
        /// </summary>
        /// <returns></returns>
        public async Task<List<PushNotificationMessagesTemplate>> GetMessagesTemplateAsync()
        {
            return await _context.PushNotificationMessagesTemplate.AsNoTracking()
                .OrderByDescending(o => o.Url)
                .ToListAsync();
        }
        /// <summary>
        /// Restituisce un messaggio in base all'Id salavato nel database
        /// </summary>
        /// <param name="messageid"></param>
        /// <returns></returns>
        public async Task<PushNotificationMessagesTemplate> GetMessageTemplateByIdAsync(int messageid)
        {
            return await _context.PushNotificationMessagesTemplate.AsNoTracking()
                .Where(o => o.MessageId == messageid)
                .SingleOrDefaultAsync();
        }
        /// <summary>
        /// Restituisce Tutti i topics utili alle sottoscrizioni
        /// </summary>
        /// <returns></returns>
        public async Task<List<PushSubscriptionTopic>> GetTopicsAsync()
        {
            return await _context.PushSubscriptionTopic.AsNoTracking()
                .OrderByDescending(o => o.TopicFilters)
                .ToListAsync();
        }
        /// <summary>
        /// Inserimento Topic nel database
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task InsertTopicAsync(PushSubscriptionTopic topic)
        {
            _context.PushSubscriptionTopic.Add(topic);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Elimina un topic dal database
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        public async Task RemoveTopicAsync(int topicid)
        {
            PushSubscriptionTopic topic = await _context.PushSubscriptionTopic.AsNoTracking().FirstOrDefaultAsync<PushSubscriptionTopic>();

            _context.PushSubscriptionTopic.Remove(topic);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Inserisce un messaggio template nel database
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task InsertMessageTemplateAsync(PushNotificationMessagesTemplate message)
        {
            _context.PushNotificationMessagesTemplate.Add(message);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Elimina un messaggio template dal database
        /// </summary>
        /// <param name="messagid"></param>
        /// <returns></returns>
        public async Task RemoveMessageTemplateAsync(int messagid)
        {
            PushNotificationMessagesTemplate message = await _context.PushNotificationMessagesTemplate.AsNoTracking().FirstOrDefaultAsync<PushNotificationMessagesTemplate>();

            _context.PushNotificationMessagesTemplate.Remove(message);

            await _context.SaveChangesAsync();
        }
    }
}
