using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Abstractions
{
    public interface IApiPushNotifications
    {
        ContentResult GetPublicKey();
        Task<IActionResult> InsertSubscription(ApiWebPushSubscription subscription);
        Task<IActionResult> RemoveSubscription(PushSubscriptionUser subscription);
        Task<IActionResult> RemoveSubscriptionByEndpoint(string endpoint);
        Task<IActionResult> InsertNotification(PushNotificationUser notification);
        Task<IActionResult> RemoveNotification(int notificationId);
        Task<IActionResult> InsertPushMessageTemplate(PushNotificationMessagesTemplate message);
        Task<IActionResult> RemovePushMessageTemplate(int messageId);
        Task<IActionResult> InsertPushTopics(PushSubscriptionTopic topic);
        Task<IActionResult> RemovePushTopic(int topicId);
        Task<ActionResult<IEnumerable<PushSubscriptionUser>>> GetSubscriptions();
        Task<ActionResult<PushSubscriptionUser>> GetSubscriptionsByEndpoint(string endpoint);
        Task<ActionResult<IEnumerable<PushNotificationUser>>> GetNotifications();
        Task<ActionResult<PushNotificationUser>> GetNotificationById(int notificationId);
        Task<IActionResult> SendWebPushNotification(WebPushNotificationMessages message);
        Task<ActionResult<IEnumerable<PushNotificationMessagesTemplate>>> GetPushMessagesTemplate();
        Task<ActionResult<PushNotificationMessagesTemplate>> GetPushMessageTemplateById(int messageId);
        Task<ActionResult<IEnumerable<PushSubscriptionTopic>>> GetTopics();
    }
}