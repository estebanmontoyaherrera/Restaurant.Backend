namespace Ordering.Application.Dtos.Dishes;

public record DishByIdResponseDto
{
    public int DishId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; } = null!;
    public bool IsAvailable { get; init; }
    public string? State { get; init; }
}
