namespace Ordering.Application.Dtos.Reports;

public record SalesReportResponseDto
{
    public decimal TotalSales { get; init; }
    public int TotalOrders { get; init; }
    public decimal AverageTicket { get; init; }
    public string? BestSellingDish { get; init; }
    public IEnumerable<SalesByCategoryDto> SalesByCategory { get; init; } = [];
    public IEnumerable<SalesByDishDto> SalesByDish { get; init; } = [];
}
