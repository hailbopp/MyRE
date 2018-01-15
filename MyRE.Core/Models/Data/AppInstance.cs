using System;
using System.Collections.Generic;

namespace MyRE.Core.Models.Data
{
    public class AppInstance
    {
        public Guid AppInstanceId { get; set; }

        public string Name { get; set; }

        public string RemoteAppId { get; set; }

        public string InstanceServerBaseUri { get; set; }

        public string AccessToken { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public List<Project> Projects { get; set; }
    }
}