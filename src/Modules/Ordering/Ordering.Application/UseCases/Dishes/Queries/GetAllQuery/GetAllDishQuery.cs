using Ordering.Application.Dtos.Dishes;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Queries.GetAllQuery;

public class GetAllDishQuery : BaseFilters, IQuery<IEnumerable<DishResponseDto>>
{
    public string? Name { get; set; }        
    public string? Category { get; set; }
  
}