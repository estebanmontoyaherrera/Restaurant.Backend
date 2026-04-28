namespace Ordering.Application.Dtos.Orders;

public record OrderByIdResponseDto
{
    public int OrderId { get; init; }
    public int TableNumber { get; init; }
    public string WaiterName { get; init; } = null!;
    public string Status { get; init; } = null!;
    public decimal Total { get; init; }
    public string? State { get; init; }
}
