using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.UseCases.Orders.Queries.GetSelectQuery;

public class GetOrderSelectQuery : IQuery<IEnumerable<SelectResponseDto>> { }
