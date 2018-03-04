using Newtonsoft.Json;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class TestProjectSourceRequest
    {
        [JsonProperty("source")]
        public string Source { get; set; }
    }
}