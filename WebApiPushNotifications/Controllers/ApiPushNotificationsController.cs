using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System;
using System.Text.Json;
using WebApiPushNotifications.Abstractions;
using WebPush;
using WebApiPushNotifications.Configurations;
using Microsoft.Extensions.Options;
using WebApiPushNotifications.Shared;
using Microsoft.AspNetCore.Authorization;
using WebApiPushNotifications.Repository;

namespace WebApiPushNotifications.Controllers
{
    //[Authorize]
    [Route("api-webpush")]
    [ApiController]
    public class ApiPushNotificationsController : ControllerBase, IApiPushNotifications
    {
        private readonly ILogger<ApiPushNotificationsController> _logger;
        private readonly WebPushNotificationConfig _configuration;
        //private readonly IWebPushStore _pushstore;
        private readonly IWebPushService _pushservice;
        private readonly IWebPushMessagesQueue _pushmessagesqueue;
        private readonly ISqlServerStoreRepository _storeRepository;

        public ApiPushNotificationsController(IOptions<WebPushNotificationConfig> configuration, ISqlServerStoreRepository storeRepository, IWebPushService pushservice, IWebPushMessagesQueue pushmessagesqueue, ILogger<ApiPushNotificationsController> logger)
        {
            this._logger = logger;
            this._configuration = configuration.Value;
            this._pushservice = pushservice;
            //this._pushstore = pushstore;
            this._pushmessagesqueue = pushmessagesqueue;
            this._storeRepository = storeRepository;
        }
        
        private string GetUserId()
        {
            if (HttpContext.User.Identity.Name != null)
            {
                return $"[Identity]={HttpContext.User.Identity.Name}";
            }
            else if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                return $"[ClaimTypes.NameIdentifier]={HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()}";
            }
            else if (HttpContext.Connection.Id != null)
            {
                return $"[ConnectionId]={HttpContext.Connection.Id.ToString()}";
            }
            else
            {
                return default;
            }
        }
        /// <summary>
        /// Api GET api-webpush/public-key
        /// </summary>
        /// <returns></returns>
        [HttpGet("public-key")]
        public ContentResult GetPublicKey()
        {
            return Content(_pushservice.PublicKey, "text/plain");
        }
        /// <summary>
        /// Api POST api-webpush/subscriptions/insert
        /// </summary>
        /// <param name="ApiWebPushSubscription->subscription"></param>
        /// <returns></returns>
        [HttpPost("subscriptions/insert")]
        public async Task<IActionResult> InsertSubscription([FromBody] ApiWebPushSubscription subscription)
        {
            if (subscription.Endpoint.Length > 0)
            {
                PushSubscriptionUser subscriptor = new PushSubscriptionUser
                {
                    UserId = GetUserId(),
                    Endpoint = subscription.Endpoint,
                    Auth = subscription.Keys["auth"],
                    P256DH = subscription.Keys["p256dh"]
                };
                // .....
                await _storeRepository.InsertSubscriptionAsync(subscriptor);

                //await _pushstore.InsertSubscriptionsAsync(subscriptor);
                return Ok();
            }
            else
            {
                return NotFound($"Subscritpion no found: {JsonSerializer.Serialize<ApiWebPushSubscription>(subscription)}");
            }
        }
        /// <summary>
        /// Api DELETE api-webpush/subscriptions/remove?endpoint={endpoint}
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpDelete("subscriptions/remove/{endpoint}")]
        public async Task<IActionResult> RemoveSubscriptionByEndpoint(string endpoint)
        {
            await _storeRepository.RemoveSubscriptionByEndpointAsync(endpoint);
            //await _pushstore.RemoveSubscriptionAsync(endpoint);
            return Ok();
        }
        /// <summary>
        /// Api REMOVE (POST) Eliminazione di una sottoscrizione
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPost("subscriptions/remove")]
        public async Task<IActionResult> RemoveSubscription([FromBody] PushSubscriptionUser subscription)
        {
            await _storeRepository.RemoveSubscriptionAsync(subscription);
            return Ok();
        }
        /// <summary>
        /// Api POST api-webpush/notifications/SendWebPushNotification
        /// </summary>
        /// <param name="WebPushNotificationMessages->message"></param>
        /// <returns></returns>
        [HttpPost("notifications/SendWebPushNotification")]
        public async Task<IActionResult> SendWebPushNotification([FromBody] WebPushNotificationMessages message)
        {
            try
            {
                await _pushmessagesqueue.Enqueue(message);
                return new OkResult();
            }
            catch (WebPushException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        /// <summary>
        /// Api GET api-webpush/notifications
        /// </summary>
        /// <returns>List<PushNotificationUser></returns>
        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<PushNotificationUser>>> GetNotifications()
        {

            return await _storeRepository.GetNotificationsAsync().ConfigureAwait(false);

            //return await _pushstore.GetNotificationsAsync();

            //.Select(o => PushNotificationWithStatus.FromExecute(o)).ToList();
        }
        /// <summary>
        /// Api GET api-webpush/notifications/{notificationId}
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>PushNotificationUser</returns>
        [HttpGet("notifications/{notificationId}")]
        public async Task<ActionResult<PushNotificationUser>> GetNotificationById(int notificationId)
        {
            var notification = await _storeRepository.GetNotificationByIdAsync(notificationId).ConfigureAwait(false);

            //var notification = await _pushstore.GetNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                return NotFound();
            }

            return notification; // PushNotificationWithStatus.FromExecute(notification);
        }
        /// <summary>
        /// Api POST api-webpush/notifications/insert
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        [HttpPost("notifications/insert")]
        public async Task<IActionResult> InsertNotification([FromBody] PushNotificationUser notification)
        {
            if (notification != null)
            {
                // .....
                await _storeRepository.InsertNotificationAsync(notification);
                //await _pushstore.InsertNotificationAsync(notification);
                return Ok();
            }
            else
            {
                return NotFound($"Notification no found: {JsonSerializer.Serialize<PushNotificationUser>(notification)}");
            }
            //return NoContent();
        }
        /// <summary>
        /// Api DELETE api-webpush/notifications/remove?{notificationId:int}
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpDelete("notifications/remove/{notificationId:int}")]
        public async Task<IActionResult> RemoveNotification(int notificationId)
        {
            PushNotificationUser notification = await _storeRepository.GetNotificationByIdAsync(notificationId);
            if(notification == null)
            {
                return NotFound();
            }

            await _storeRepository.RemoveNotificationAsync(notification);
            return Ok();
        }
        /// <summary>
        /// Api POST api-webpush/pushmessage-template/insert
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>    
        [HttpPost("pushmessage-template/insert")]
        public async Task<IActionResult> InsertPushMessageTemplate([FromBody] PushNotificationMessagesTemplate message)
        {
            try
            {
                await _storeRepository.InsertMessageTemplateAsync(message);
                return Ok();
            }
            catch (Exception ex)
            { 
                return NotFound($"Subscritpion no found: {JsonSerializer.Serialize<PushNotificationMessagesTemplate>(message)}");
            }
        }
        /// <summary>
        /// Api DELETE api-webpush/pushmessage-template/remove/{messageId:int}
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpDelete("pushmessage-template/remove/{messageId:int}")]
        public async Task<IActionResult> RemovePushMessageTemplate(int messageId)
        {
            PushNotificationMessagesTemplate message = await _storeRepository.GetMessageTemplateByIdAsync(messageId);
            if(message == null)
            {
                return NotFound();
            }
            await _storeRepository.RemoveMessageTemplateAsync(message);
            return Ok();
        }
        /// <summary>
        /// Api POST api-webpush/subcriptions-topic/insert
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost("subscriptions-topic/insert")]
        public async Task<IActionResult> InsertPushTopics([FromBody] PushSubscriptionTopic topic)
        {
            try
            {
                await _storeRepository.InsertSubscriptionTopicsAsync(topic);
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound($"Topic no found: {JsonSerializer.Serialize<PushSubscriptionTopic>(topic)}");
            }
        }
        /// <summary>
        /// Api DELETE api-webpush/pushtopic/remove?{topicId}
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpDelete("subscriptions-topic/remove/{topicId:int}")]
        public async Task<IActionResult> RemovePushTopic(int topicId)
        {
            PushSubscriptionTopic topic = await _storeRepository.GetSubscriptionTopicsByIdAsync(topicId);
            if (topic == null)
            {
                return NotFound();
            }
            await _storeRepository.InsertSubscriptionTopicsAsync(topic);
            return Ok();
        }
        /// <summary>
        /// Api GET api-webpush/subcriptions-topic/topics
        /// </summary>
        /// <returns></returns>
        [HttpGet("subscriptions-topic/topics")]
        public async Task<ActionResult<IEnumerable<PushSubscriptionTopic>>> GetTopics()
        {
            return await _storeRepository.GetSubscriptionTopicsAsync();
        }
        /// <summary>
        /// Api GET api-webpush/subscriptions
        /// </summary>
        /// <returns></returns>
        [HttpGet("subscriptions")]
        public async Task<ActionResult<IEnumerable<PushSubscriptionUser>>> GetSubscriptions()
        {
            return await _storeRepository.GetSubscriptionsAsync();
        }
        /// <summary>
        /// Api GET api-webpush/subscriptions/{endpoint}
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet("subscriptions/{endpoint}")]
        public async Task<ActionResult<PushSubscriptionUser>> GetSubscriptionsByEndpoint(string endpoint)
        {
            return await _storeRepository.GetSubscriptionByEndpointAsync(endpoint);
        }
        /// <summary>
        /// Api GET api-webpush/pushmessage-template/messages 
        /// </summary>
        /// <returns></returns>
        [HttpGet("pushmessage-template/messages")]
        public async Task<ActionResult<IEnumerable<PushNotificationMessagesTemplate>>> GetPushMessagesTemplate()
        {
            return await _storeRepository.GetMessagesTemplateAsync();
        }
        /// <summary>
        /// Api GET api-webpush/pushmessage-template/message/{messageId:int}
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet("pushmessage-template/message/{messageId:int}")]
        public async Task<ActionResult<PushNotificationMessagesTemplate>> GetPushMessageTemplateById(int messageId)
        {
            return await _storeRepository.GetMessageTemplateByIdAsync(messageId);
        }

    }
}
