using System;
using System.Collections.Generic;

namespace MyRE.Core.Models.Data
{
    public class Account
    {
        public Guid AccountId { get; set; }
        
        public string RemoteAccountId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public IEnumerable<AppInstance> AppInstances { get; set; }
    }
}
