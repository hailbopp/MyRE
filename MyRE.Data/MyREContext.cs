using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Models.Data;

namespace MyRE.Data
{
    public class MyREContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppInstance> AppInstances { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSource> ProjectSourceVersions { get; set; }

        public MyREContext(DbContextOptions<MyREContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            MyREConfiguration.OnModelCreating(builder);
        }
    }
}
