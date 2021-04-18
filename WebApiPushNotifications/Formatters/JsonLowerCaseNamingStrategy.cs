using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPushNotifications.Formatters
{
    public class JsonLowerCaseNamingStrategy: NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return name.ToLower();
        }
    }
}
