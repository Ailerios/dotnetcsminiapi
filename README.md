# Orders API

Mini Orders API demo designed to demonstrate C#/.NET backend fundamentals: **ASP.NET Core Minimal API** with **Swagger**, **EF Core (SQLite)**, **xUnit tests**, **Dockerfile**, and **GitHub Actions CI**.

## Quickstart

### Prereqs
- .NET SDK 8.0+
- (Optional) Docker
- (Optional) `dotnet-ef` CLI: `dotnet tool install --global dotnet-ef`

### Restore & Build
```bash
dotnet restore
dotnet build --configuration Release
```

### Create DB (SQLite) & Run
```bash
# Add the first migration (one-time, creates the Migrations folder)
dotnet ef migrations add InitialCreate --project src/Orders.Api

# Apply migrations (creates orders.db)
dotnet ef database update --project src/Orders.Api

# Run the API
dotnet run --project src/Orders.Api
# Swagger/OpenAPI UI at http://localhost:5000/swagger
```

### Example requests
```bash
curl "http://localhost:5000/api/orders?page=1&pageSize=5&status=Pending"
curl -X POST "http://localhost:5000/api/orders" -H "Content-Type: application/json" -d '{"customerName":"ACME","total":123.45,"status":"Pending"}'
```

### Tests
```bash
dotnet test
```

### Docker
```bash
# Build
docker build -t orders-api:latest .

# Run
docker run -p 5000:8080 orders-api:latest
# Swagger: http://localhost:5000/swagger
```

## Project structure
```
src/Orders.Api/
  Data/AppDbContext.cs        # EF Core context
  Data/Seed.cs                # Simple data seeding
  Models/Order.cs             # Entity
  Models/OrderDtos.cs         # Create/Update DTOs
  Program.cs                  # Minimal API endpoints + DI + Swagger
  appsettings.json            # Connection string (SQLite)

tests/Orders.Api.Tests/
  CustomWebAppFactory.cs      # In-memory test host + EF InMemory swap + seeding
  OrdersEndpointsTests.cs     # Basic GET/POST tests

Dockerfile                    # Multi-stage build
.github/workflows/ci.yml      # Build & test pipeline
```

## Notes
- Program.cs seeds a few sample orders on first run.
- Tests replace the SQLite DB with EF Core InMemory provider and seed test data.

## License
MIT