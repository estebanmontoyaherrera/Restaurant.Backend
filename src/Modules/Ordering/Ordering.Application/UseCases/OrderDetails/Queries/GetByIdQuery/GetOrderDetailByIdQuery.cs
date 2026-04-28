using Ordering.Application.Dtos.OrderDetails;
using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetByIdQuery;

public class GetOrderDetailByIdQuery : IQuery<OrderDetailByIdResponseDto>
{
    public int OrderDetailId { get; set; }
}
