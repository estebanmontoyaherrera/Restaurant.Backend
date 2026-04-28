namespace Ordering.Application.Dtos.Reports;

public record SalesByCategoryDto
{
    public string Category { get; init; } = null!;
    public int QuantitySold { get; init; }
    public decimal TotalSales { get; init; }
    public decimal PercentageOfTotal { get; init; }
}
