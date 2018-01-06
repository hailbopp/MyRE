using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRE.Web.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyRE.Web.Data
{
    public class MyREContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppInstance> AppInstances { get; set; }
        public DbSet<OAuthInfo> OAuthInfo { get; set; }

        public MyREContext(DbContextOptions<MyREContext> options)
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
