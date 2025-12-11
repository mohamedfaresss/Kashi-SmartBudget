using Kashi.Domain;
using Kashi_SmartBudget.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kashi_SmartBudget.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Budget> Budgets { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
          base.OnModelCreating(builder);

            builder.Entity<Account>()
                .Property(a => a.Currency)
                .HasMaxLength(10);
            builder.Entity<Category>()
                .HasIndex(c => new { c.UserId, c.Name });
            builder.Entity<Transaction>()
                .HasIndex(t => t.UserId);
            builder.Entity<Budget>()
                .HasIndex(b => b.UserId);
                



        }




    }
    
}
