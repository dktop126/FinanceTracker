using Microsoft.EntityFrameworkCore;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Account>().Property(t => t.Balance).HasPrecision(18, 2);
    }
}