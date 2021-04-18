
namespace WebApiPushNotifications.Shared
{
    //
    // Riepilogo:
    //     The push message urgency.
    public enum MessageUrgency
    {
        //
        // Riepilogo:
        //     Very low (e.g. advertisements).
        VeryLow = 0,
        //
        // Riepilogo:
        //     Low (e.g. topic updates).
        Low = 1,
        //
        // Riepilogo:
        //     Normal (e.g. chat message).
        Normal = 2,
        //
        // Riepilogo:
        //     High (e.g. time-sensitive alert).
        High = 3
    }
    public class ApiPushMessage
    {
        public string Topic { get; set; }

        public string Notification { get; set; }

        public MessageUrgency Urgency { get; set; } = MessageUrgency.Normal;
    }
}
