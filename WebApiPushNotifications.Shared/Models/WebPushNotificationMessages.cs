
namespace WebApiPushNotifications.Shared
{
    public class WebPushNotificationAction
    {
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        
        public WebPushNotificationAction(string action, string title, string icon)
        {
            this.Action = action;
            this.Title = title;
            this.Icon = icon;
        }
    }

    public class WebPushNotificationMessagesImage : WebPushNotificationMessages
    {
        public string Badge { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        
    }

    public class WebPushNotificationMessages
    {
        public string Message { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public WebPushNotificationAction[] Actions { get; set; }
        public bool RequireInteraction { get; set; } = true;
        public int[] Vibrate { get; set; } = new int[] { 100, 50, 100 };

    }
}
