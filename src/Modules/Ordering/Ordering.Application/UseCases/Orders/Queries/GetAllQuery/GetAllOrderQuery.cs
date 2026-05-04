using Ordering.Application.Dtos.Orders;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Queries.GetAllQuery;

public class GetAllOrderQuery : BaseFilters, IQuery<IEnumerable<OrderResponseDto>> {

    public string? StatusFilter { get; set; } 
}
