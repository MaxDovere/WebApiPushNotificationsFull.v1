
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiPushNotifications.Shared
{
    public class PushSubscriptionUser
    {
        [Key] public int SubscriptionId { get; set; }
        [Required] public string Endpoint { get; set; }
        [Required] public string UserId { get; set; }
        public PushSubscriptionTopic TopicKeys { get; set; }
        [Required] public string P256DH { get; set; }
        [Required] public string Auth { get; set; }
        [Required] public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime DateEndTime { get; set; }

    }

    public class PushSubscriptionTopic
    {
        [Key] public int TopicId { get; set; }
        [Required] public string TopicFilters { get; set; } = "";
        public bool Actived { get; set; } = false;
        [Required] public TimeSpan TimeAfterToSend { get; set; }
    }

}
