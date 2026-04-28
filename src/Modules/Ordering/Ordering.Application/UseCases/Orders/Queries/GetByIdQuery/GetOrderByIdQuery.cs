using Ordering.Application.Dtos.Orders;
using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Orders.Queries.GetByIdQuery;

public class GetOrderByIdQuery : IQuery<OrderByIdResponseDto>
{
    public int OrderId { get; set; }
}
