using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyRE.Core.Models.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
}
