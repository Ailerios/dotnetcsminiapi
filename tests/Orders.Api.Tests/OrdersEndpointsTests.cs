using System.Net;
using System.Net.Http.Json;

public class OrdersEndpointsTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;

    public OrdersEndpointsTests(CustomWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_returns_ok_and_items()
    {
        var resp = await _client.GetAsync("/api/orders?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var payload = await resp.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(payload);
    }

    [Fact]
    public async Task PostOrder_creates_resource()
    {
        var newOrder = new { customerName = "Rocket Lab", total = 42.5m, status = "Pending" };
        var resp = await _client.PostAsJsonAsync("/api/orders", newOrder);
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
    }
}
