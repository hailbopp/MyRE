using System.Collections.Generic;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class AttributeInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}