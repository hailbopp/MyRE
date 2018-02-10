using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRE.Core.Models.Data
{
    public class Account
    {
        public Guid AccountId { get; set; }
        
        public string RemoteAccountId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        public IEnumerable<AppInstance> AppInstances { get; set; }
    }
}
