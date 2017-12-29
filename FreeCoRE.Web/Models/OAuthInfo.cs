using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCoRE.Web.Models
{
    public class OAuthInfo
    {
        public long OAuthInfoId { get; set; }
        public Account Account { get; set; }
        public string ClientId {get;set;}
        public string ClientSecret { get; set; }
        public string AuthorizationCode { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset TokenExpiration { get; set; }
    }
}
