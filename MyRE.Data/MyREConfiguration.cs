using Microsoft.EntityFrameworkCore;
using MyRE.Core.Models;

namespace MyRE.Data
{
    internal static class MyREConfiguration
    {
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasAlternateKey(i => i.RemoteAccountId)
                .HasName("UNQ_RemoteAccountId");

            builder.Entity<AppInstance>()
                .HasAlternateKey(i => i.RemoteAppId)
                .HasName("UNQ_RemoteAppId");
        }
    }
}