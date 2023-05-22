using ElderEatsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderEatsAPI.Data;

public class ElderEatsContext : DbContext
{
    public ElderEatsContext(DbContextOptions<ElderEatsContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Account> Accounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Account>().ToTable("accounts");
    }
}
