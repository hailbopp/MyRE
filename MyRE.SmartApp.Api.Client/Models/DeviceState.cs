using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class DeviceState
    {
        public string DeviceId { get; set; }
        public string Label { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<AttributeState> AttributeStates { get; set; }
    }
}