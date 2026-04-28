using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetSelectQuery;

public class GetOrderDetailSelectQuery : IQuery<IEnumerable<SelectResponseDto>> { }
