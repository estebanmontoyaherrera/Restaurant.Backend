namespace Ordering.Application.Dtos.OrderDetails;

public record OrderDetailByIdResponseDto
{
    public int OrderDetailId { get; init; }
    public int OrderId { get; init; }
    public int DishId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Subtotal { get; init; }
    public string? Notes { get; init; }
    public string? State { get; init; }
}
