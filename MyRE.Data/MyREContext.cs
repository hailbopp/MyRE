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
        public DbSet<Routine> Routines { get; set; }

        public DbSet<Expression> Expressions { get; set; }
        public DbSet<LiteralExpression> LiteralExpressions { get; set; }
        public DbSet<VariableNameExpression> VariableNameExpressions { get; set; }
        public DbSet<InvocationExpression> InvocationExpressions { get; set; }

        public DbSet<Statement> Statements { get; set; }
        public DbSet<EventHandlerStatement> EventHandlerStatements { get; set; }
        public DbSet<IfStatement> IfStatements { get; set; }
        public DbSet<WhileStatement> WhileStatements { get; set; }
        public DbSet<VariableDefinitionStatement> VariableDefinitionStatements { get; set; }
        public DbSet<VariableAssignmentStatement> VariableAssignmentStatements { get; set; }
        public DbSet<ActionStatement> ActionStatements { get; set; }


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
