using Ordering.Application.Dtos.OrderDetails;
using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetByIdQuery;

public class GetOrderDetailsByOrderIdQuery : IQuery<IEnumerable<OrderDetailResponseDto>>
{
    public int OrderId { get; set; }
}