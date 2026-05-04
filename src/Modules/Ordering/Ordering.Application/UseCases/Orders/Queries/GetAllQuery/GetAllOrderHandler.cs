using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.Orders;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Ordering.Application.UseCases.Orders.Queries.GetAllQuery;

public class GetAllOrderHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery)
    : IQueryHandler<GetAllOrderQuery, IEnumerable<OrderResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<OrderResponseDto>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<OrderResponseDto>>();

        try
        {
            var data = _unitOfWork.Orders.GetAllQueryable();

            // 🔹 FILTRO ORIGINAL (STATE - NO TOCAR)
            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                data = data.Where(x => stateFilter.Contains(x.State));
            }

            // 🔹 NUEVO FILTRO POR STATUS
            if (!string.IsNullOrWhiteSpace(request.StatusFilter))
            {
                var statusFilter = request.StatusFilter.Split('-');
                data = data.Where(x => statusFilter.Contains(x.Status));
            }

            // 🔹 FECHAS
            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                data = data.Where(x =>
                    x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                    x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1));
            }

            request.Sort ??= "Id";

            var items = await _orderingQuery.Ordering(request, data)
                .ToListAsync(cancellationToken);

            var orderIds = items.Select(x => x.Id).ToList();

            var totalsByOrder = await _unitOfWork.OrderDetails.GetAllQueryable()
                .Where(x => orderIds.Contains(x.OrderId))
                .GroupBy(x => x.OrderId)
                .Select(g => new
                {
                    OrderId = g.Key,
                    Total = g.Sum(d => d.Quantity * d.UnitPrice)
                })
                .ToDictionaryAsync(x => x.OrderId, x => x.Total, cancellationToken);

            var dtos = items.Adapt<List<OrderResponseDto>>();

            dtos = dtos
                .Select(x => x with
                {
                    Total = totalsByOrder.TryGetValue(x.OrderId, out var total)
                        ? total
                        : 0m
                })
                .ToList();

            response.IsSuccess = true;
            response.TotalRecords = await data.CountAsync(cancellationToken);
            response.Data = dtos;
            response.Message = "Query successful.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}