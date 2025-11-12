namespace Orders.Api.Models;

public enum OrderStatus { Pending, Processing, Completed, Cancelled }

public sealed class Order
{
  public int Id { get; set; }
  public string CustomerName { get; set; } = string.Empty;
  public decimal Total { get; set; }
  public OrderStatus Status { get; set; } = OrderStatus.Pending;
  public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}