using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRE.Web.Data.Models
{
    public class Account
    {
        public string AccountId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
