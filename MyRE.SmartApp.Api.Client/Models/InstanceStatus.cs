using System;
using System.Collections.Generic;
using System.Text;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class InstanceStatus
    {
        public string InstanceId { get; set; }
        public string AccountId { get; set; }
        public string InstanceName { get; set; }
    }

    public class AttributeInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Values { get; set; }
    }

    public class ArgumentDataType
    {
        public string EnumType { get; set; }
        public string Name { get; set; }
    }

    public class CommandInfo
    {
        public string Name { get; set; }
        public IEnumerable<ArgumentDataType> Arguments { get; set; }
    }

    public class DeviceInfo
    {
        public string DeviceId { get; set; }
        public string Label { get; set; }
        public string DisplayName { get; set; }
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }

        public IEnumerable<AttributeInfo> Attributes { get; set; }
        public IEnumerable<CommandInfo> Commands { get; set; }
    }
}
