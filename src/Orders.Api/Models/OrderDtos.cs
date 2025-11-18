namespace Orders.Api.Models;

public record OrderCreateDto(string CustomerName, decimal Total, string? Status);
public record OrderUpdateDto(string? CustomerName, decimal? Total, string? Status);