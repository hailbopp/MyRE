namespace MyRE.Web.Data.Models
{
    public class AppInstance
    {        
        public string AppInstanceId { get; set; }

        public string InstanceServerBaseUri { get; set; }

        public string AccessToken { get; set; }

        public string ExecutionToken { get; set; }

        public Account Account { get; set; }
    }
}