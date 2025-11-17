using Orders.Api.Models;

namespace Orders.Api.Data;

public static class Seed
{
    public static async Task EnsureSeedData(AppDbContext db)
    {
        if (db.Orders.Any()) return;
        db.Orders.AddRange(
            new Order { CustomerName="ACME", Total=199.99m, Status="Pending", CreatedAtUtc=DateTime.UtcNow.AddDays(-2)},
            new Order { CustomerName="Wayne Enterprises", Total=599.00m, Status="Shipped", CreatedAtUtc=DateTime.UtcNow.AddDays(-1)},
            new Order { CustomerName="Stark Industries", Total=1299.50m, Status="Pending", CreatedAtUtc=DateTime.UtcNow}
        );
        await db.SaveChangesAsync();
    }
}
