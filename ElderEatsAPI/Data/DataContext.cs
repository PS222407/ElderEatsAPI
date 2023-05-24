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
    
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<AccountProduct> AccountProducts { get; set; }
    
    public DbSet<AccountUser> AccountUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Account>().ToTable("accounts");
        modelBuilder.Entity<User>().ToTable("users");

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

        modelBuilder.Entity<AccountUser>().ToTable("account_users")
            .HasKey(au => new { au.AccountId, au.UserId });
        modelBuilder.Entity<AccountUser>().ToTable("account_users")
            .HasOne(au => au.Account)
            .WithMany(a => a.AccountUsers)
            .HasForeignKey(au => au.AccountId);
        modelBuilder.Entity<AccountUser>().ToTable("account_users")
            .HasOne(au => au.User)
            .WithMany(u => u.AccountUsers)
            .HasForeignKey(au => au.UserId);
    }
}
