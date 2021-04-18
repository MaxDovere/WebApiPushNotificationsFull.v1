using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiPushNotifications.Shared
{
    public class ApiWebPushSubscription
    {
        public string Endpoint { get; set; }

        public IDictionary<string, string> Keys { get; set; }
    }
}
