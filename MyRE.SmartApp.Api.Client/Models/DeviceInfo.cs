using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class DeviceInfo
    {
        public string DeviceId { get; set; }
        public string Label { get; set; }
        public string DisplayName { get; set; }
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }

        public IEnumerable<AttributeInfo> Attributes { get; set; }
        public IEnumerable<CommandInfo> Commands { get; set; }
        public IEnumerable<CapabilityInfo> Capabilities { get; set; }
    }
}