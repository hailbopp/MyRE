using Newtonsoft.Json;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class CreateChildAppRequest : BaseChildAppPropertiesRequest
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }
    }
}