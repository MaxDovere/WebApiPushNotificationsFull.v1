using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiPushNotifications.Shared
{
    public class PushNotificationMessagesTemplate
    {
        [Key] public int MessageId { get; set; }
        [Required] public PushNotificationMessageType MessageType { get; set; }
        public string Title { get; set; }
        [Required] public string Message { get; set; }
        [Required] public string Body { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public string Data { get; set; }
        public bool RequireInteraction { get; set; }
        public string Vibrate { get; set; }
        public List<PushNotificationMessageActions> MessageActions { get; set; }

        #region Image
        public string Badge { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        #endregion

        public PushNotificationMessagesTemplate()
        {
            MessageActions = new List<PushNotificationMessageActions>();
        }
    }

    public class PushNotificationMessageActions
    {
        [Key] public int MessageActionId { get; set; }
        public PushNotificationMessagesTemplate Message { get; set; }
        public PushNotificationMessageAction Action { get; set; }
    }

    public class PushNotificationMessageAction
    {
        [Key] public int ActionId { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
    public class PushNotificationMessageType
    {
        [Key] public int MessageTypeId { get; set; }
        [Required] public string Name { get; set; } = "Tile";
        public string Description { get; set; }
        public PushNotificationMessageUrgency Urgency { get; set; }
    }
    
    public class PushNotificationMessageUrgency
    {
        public int UrgencyId { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
    }
}
