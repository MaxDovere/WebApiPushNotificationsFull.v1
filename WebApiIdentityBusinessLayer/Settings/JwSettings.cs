using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiIdentityServer.BusinessLayer.Settings
{
    public class JwtSettings
    {
        public string SecurityKey { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; set; }
    }
}
