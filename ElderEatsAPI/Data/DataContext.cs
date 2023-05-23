using ElderEatsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderEatsAPI.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Account> Accounts { get; set; } = null!;

    public DbSet<AccountProduct> AccountProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Account>().ToTable("accounts");

        modelBuilder.Entity<AccountProduct>().ToTable("account_products")
            .HasKey(ap => new { ap.AccountId, ap.ProductId });
        modelBuilder.Entity<AccountProduct>().ToTable("account_products")
            .HasOne(a => a.Account)
            .WithMany(ap => ap.AccountProducts)
            .HasForeignKey(a => a.AccountId);
        modelBuilder.Entity<AccountProduct>().ToTable("account_products")
            .HasOne(a => a.Product)
            .WithMany(ap => ap.AccountProducts)
            .HasForeignKey(p => p.ProductId);
    }
}
