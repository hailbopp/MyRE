using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCoRE.Web.Models
{
    public class FreeCoreContext : DbContext
    {
        public FreeCoreContext(DbContextOptions<FreeCoreContext> options) : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppInstance> AppInstances { get; set; }
    }
}
