namespace Ordering.Application.Dtos.Reports;

public record SalesByDishDto
{
    public string Name { get; init; } = null!;
    public string Category { get; init; } = null!;
    public int QuantitySold { get; init; }
    public decimal TotalSales { get; init; }
}
