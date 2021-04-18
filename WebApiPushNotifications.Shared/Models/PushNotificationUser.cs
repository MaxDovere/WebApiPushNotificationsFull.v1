using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiPushNotifications.Shared
{
    public class PushNotificationUser
    {
        [Key] public int NotificationId { get; set; }

        [Required] public string UserId { get; set; }

        [Required] public string Topic { get; set; }

        [Required] public DateTime CreatedTime { get; set; } = DateTime.Now;

        public int SubscriptionId { get; set; }

        public string NotificationMessage { get; set; }

    }
}
