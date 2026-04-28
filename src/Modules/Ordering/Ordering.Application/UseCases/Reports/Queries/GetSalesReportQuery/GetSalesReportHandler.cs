using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.Reports;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Reports.Queries.GetSalesReportQuery;

public class GetSalesReportHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetSalesReportQuery, SalesReportResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<SalesReportResponseDto>> Handle(GetSalesReportQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<SalesReportResponseDto>();

        try
        {
            var startDate = string.IsNullOrWhiteSpace(request.StartDate)
                ? (DateTime?)null
                : DateTime.SpecifyKind(Convert.ToDateTime(request.StartDate).Date, DateTimeKind.Utc);

            var endDate = string.IsNullOrWhiteSpace(request.EndDate)
                ? (DateTime?)null
                : DateTime.SpecifyKind(Convert.ToDateTime(request.EndDate).Date.AddDays(1), DateTimeKind.Utc);

            var salesOrders = _unitOfWork.Orders.GetAllQueryable()
                .Where(x => x.Status == "Entregado" || x.Status == "Cerrado");

            if (startDate.HasValue)
                salesOrders = salesOrders.Where(x => x.AuditCreateDate >= startDate.Value);

            if (endDate.HasValue)
                salesOrders = salesOrders.Where(x => x.AuditCreateDate < endDate.Value);

            var salesOrderIdsQuery = salesOrders.Select(x => x.Id);

            var detailQuery = _unitOfWork.OrderDetails.GetAllQueryable()
                .Where(x => salesOrderIdsQuery.Contains(x.OrderId));

            var totalSales = await detailQuery.SumAsync(x => (decimal?)(x.Quantity * x.UnitPrice), cancellationToken) ?? 0m;
            var totalOrders = await salesOrders.CountAsync(cancellationToken);
            var averageTicket = totalOrders == 0 ? 0m : totalSales / totalOrders;

            var bestSellingDishData = await detailQuery
                .GroupBy(x => x.DishId)
                .Select(g => new { DishId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Quantity)
                .FirstOrDefaultAsync(cancellationToken);

            var bestSellingDish = bestSellingDishData is null
                ? null
                : await _unitOfWork.Dishes.GetAllQueryable()
                    .Where(x => x.Id == bestSellingDishData.DishId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync(cancellationToken);

            var salesByCategoryData = await detailQuery
                .Join(_unitOfWork.Dishes.GetAllQueryable(),
                    detail => detail.DishId,
                    dish => dish.Id,
                    (detail, dish) => new { dish.Category, detail.Quantity, detail.UnitPrice })
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync(cancellationToken);

            var salesByDish = await detailQuery
                .Join(_unitOfWork.Dishes.GetAllQueryable(),
                    detail => detail.DishId,
                    dish => dish.Id,
                    (detail, dish) => new { dish.Name, dish.Category, detail.Quantity, detail.UnitPrice })
                .GroupBy(x => new { x.Name, x.Category })
                .Select(g => new SalesByDishDto
                {
                    Name = g.Key.Name,
                    Category = g.Key.Category,
                    QuantitySold = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync(cancellationToken);

            var salesByCategory = salesByCategoryData.Select(x => new SalesByCategoryDto
            {
                Category = x.Category,
                QuantitySold = x.QuantitySold,
                TotalSales = x.TotalSales,
                PercentageOfTotal = totalSales == 0 ? 0 : Math.Round((x.TotalSales / totalSales) * 100, 2)
            }).ToList();

            response.IsSuccess = true;
            response.Message = "Consulta exitosa.";
            response.Data = new SalesReportResponseDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                AverageTicket = averageTicket,
                BestSellingDish = bestSellingDish,
                SalesByCategory = salesByCategory,
                SalesByDish = salesByDish
            };
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
