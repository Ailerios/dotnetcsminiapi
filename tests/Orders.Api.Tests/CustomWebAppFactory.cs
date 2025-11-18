using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Api.Data;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // remove the real DbContext registration (SQLite)
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(descriptor);

            // add InMemory DbContext for tests
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("OrdersTestDb"));

            // build + seed
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            if (!db.Orders.Any())
            {
                db.Orders.AddRange(
                    new Orders.Api.Models.Order { CustomerName="TestCo", Total=10, Status="Pending" },
                    new Orders.Api.Models.Order { CustomerName="Widgets Ltd", Total=20, Status="Shipped" }
                );
                db.SaveChanges();
            }
        });
    }
}
