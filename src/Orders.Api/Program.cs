using Microsoft.EntityFrameworkCore;
using Orders.Api.Data;
using Orders.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core (SQLite) ---
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// --- Swagger/OpenAPI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await Seed.EnsureSeedData(db);
}

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// List with paging + optional status filter
app.MapGet("/api/orders", async (AppDbContext db, int page = 1, int pageSize = 10, string? status = null) =>
{
    if (page < 1 || pageSize < 1 || pageSize > 100) return Results.BadRequest("Invalid paging");

    var q = db.Orders.AsQueryable();
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(o => o.Status == status);

    var total = await q.CountAsync();
    var items = await q.OrderByDescending(o => o.CreatedAtUtc)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

    return Results.Ok(new { total, items });
});

// Get by id
app.MapGet("/api/orders/{id:int}", async (int id, AppDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    return order is null ? Results.NotFound() : Results.Ok(order);
});

// Create
app.MapPost("/api/orders", async (AppDbContext db, OrderCreateDto input) =>
{
    if (string.IsNullOrWhiteSpace(input.CustomerName) || input.Total < 0)
        return Results.BadRequest("Invalid payload");

    var order = new Order
    {
        CustomerName = input.CustomerName,
        Total = input.Total,
        Status = string.IsNullOrWhiteSpace(input.Status) ? "Pending" : input.Status,
        CreatedAtUtc = DateTime.UtcNow
    };
    db.Orders.Add(order);
    await db.SaveChangesAsync();
    return Results.Created($"/api/orders/{order.Id}", order);
});

// Update
app.MapPut("/api/orders/{id:int}", async (int id, AppDbContext db, OrderUpdateDto input) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order is null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(input.CustomerName)) order.CustomerName = input.CustomerName!;
    if (input.Total is not null && input.Total.Value >= 0) order.Total = input.Total.Value;
    if (!string.IsNullOrWhiteSpace(input.Status)) order.Status = input.Status!;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete
app.MapDelete("/api/orders/{id:int}", async (int id, AppDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order is null) return Results.NotFound();
    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Simple health check
app.MapGet("/", () => Results.Ok("OK"));

app.Run();

public partial class Program { } // For integration testing