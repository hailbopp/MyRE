using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCoRE.Web.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeCoRE.Web.Data
{
    public class FreeCoreContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppInstance> AppInstances { get; set; }
        public DbSet<OAuthInfo> OAuthInfo { get; set; }

        public FreeCoreContext(DbContextOptions<FreeCoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
