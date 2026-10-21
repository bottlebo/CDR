using CDR_Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CDR_Exchange.Dal;

public class ExchangeDbContext : DbContext
{
    public ExchangeDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TransitWallet> TransitWallets => Set<TransitWallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionHistory> TransactionHistories => Set<TransactionHistory>();
    public DbSet<Rate> Rates => Set<Rate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<TransitWallet>().HasQueryFilter(p => DateTime.UtcNow.AddHours(-24) < p.CreateDate);
    }
}