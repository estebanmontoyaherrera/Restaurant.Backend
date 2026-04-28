using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.UseCases.Dishes.Queries.GetSelectQuery;

public class GetDishSelectQuery : IQuery<IEnumerable<SelectResponseDto>> { }
