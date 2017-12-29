using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCoRE.Web.Models
{
    public class Account
    {
        public string AccountId { get; set; }

        public ApplicationUser User { get; set; }
    }

    public class AppInstance
    {        
        public string AppInstanceId { get; set; }
        public string LocationId { get; set; }
        public string Region { get; set; }

        public Account Account { get; set; }
    }
}
