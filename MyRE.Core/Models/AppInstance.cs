namespace MyRE.Core.Models
{
    public class AppInstance
    {
        public long AppInstanceId { get; set; }

        public string RemoteAppId { get; set; }

        public string InstanceServerBaseUri { get; set; }

        public string AccessToken { get; set; }

        public long AccountId { get; set; }
        public Account Account { get; set; }
    }
}