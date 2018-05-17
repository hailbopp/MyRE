using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class ExecuteDeviceCommandRequest
    {
        [JsonProperty("params")]
        public IEnumerable<object> Parameters {get; set;}
    }
}
