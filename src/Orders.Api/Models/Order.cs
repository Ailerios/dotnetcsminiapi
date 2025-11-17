namespace Orders.Api.Models;

public sealed class Order
{
  public int Id { get; set; }
  public string CustomerName { get; set; } = string.Empty;
  public decimal Total { get; set; }
  public string Status { get; set; } = "Pending";
  public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}