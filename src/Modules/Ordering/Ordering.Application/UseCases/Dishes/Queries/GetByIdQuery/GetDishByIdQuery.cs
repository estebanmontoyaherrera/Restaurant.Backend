using Ordering.Application.Dtos.Dishes;
using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Dishes.Queries.GetByIdQuery;

public class GetDishByIdQuery : IQuery<DishByIdResponseDto>
{
    public int DishId { get; set; }
}
