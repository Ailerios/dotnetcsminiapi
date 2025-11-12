using Microsoft.EntityFrameworkCore;
using Orders.Api.Models;

namespace Orders.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Order> Orders => Set<Order>();

  protected override void OnModelCreating(ModelBuilder b)
  {
    b.Entity<Order>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
      e.Property(x => x.Total).HasColumnType("decimal(18,2)");
      e.Property(x => x.CreatedAtUtc).HasConversion(
              v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
      // Order.OrderStatus (enum) maps to int by default
    });
  }
}
