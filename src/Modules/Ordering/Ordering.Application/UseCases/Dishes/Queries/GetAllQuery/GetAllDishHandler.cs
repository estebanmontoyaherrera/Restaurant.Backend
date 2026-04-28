using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.Dishes;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Ordering.Application.UseCases.Dishes.Queries.GetAllQuery;

public class GetAllDishHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery) : IQueryHandler<GetAllDishQuery, IEnumerable<DishResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<DishResponseDto>>> Handle(GetAllDishQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<DishResponseDto>>();

        try
        {
            var dishes = _unitOfWork.Dishes.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(request.Category))
                dishes = dishes.Where(x => x.Category == request.Category);

            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                dishes = dishes.Where(x => stateFilter.Contains(x.State));
            }

            request.Sort ??= "Category";
            var ordered = dishes.OrderBy(x => x.Category).ThenBy(x => x.Name);
            var items = await _orderingQuery.Ordering(request, ordered).ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await dishes.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<DishResponseDto>>();
            response.Message = "Query successful.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
