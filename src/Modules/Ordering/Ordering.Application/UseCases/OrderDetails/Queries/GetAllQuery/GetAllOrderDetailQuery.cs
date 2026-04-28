using Ordering.Application.Dtos.OrderDetails;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetAllQuery;

public class GetAllOrderDetailQuery : BaseFilters, IQuery<IEnumerable<OrderDetailResponseDto>> { }
