using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class CapabilityInfo
    {
        public string Name { get; set; }
        public IEnumerable<AttributeInfo> Attributes { get; set; }
        public IEnumerable<CommandInfo> Commands { get; set; }
    }
}