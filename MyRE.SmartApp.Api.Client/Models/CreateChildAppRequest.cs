using Newtonsoft.Json;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class CreateChildAppRequest
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}