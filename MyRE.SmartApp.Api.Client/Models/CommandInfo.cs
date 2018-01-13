using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class CommandInfo
    {
        public string Name { get; set; }
        public IEnumerable<ArgumentDataType> Arguments { get; set; }
    }
}