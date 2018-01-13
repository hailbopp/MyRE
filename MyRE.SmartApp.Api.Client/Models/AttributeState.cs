using System;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class AttributeState
    {
        public string Name { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Value { get; set; }
    }
}