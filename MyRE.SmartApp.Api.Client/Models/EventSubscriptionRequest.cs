using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class EventSubscriptionRequest
    {
        public IEnumerable<string> DeviceIds { get; set; }
        public string EventName { get; set; }
        
    }
}