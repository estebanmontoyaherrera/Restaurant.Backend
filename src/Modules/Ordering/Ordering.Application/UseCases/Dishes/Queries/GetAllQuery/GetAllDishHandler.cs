using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.Dishes;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Ordering.Application.UseCases.Dishes.Queries.GetAllQuery;

public class GetAllDishHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery) 
    : IQueryHandler<GetAllDishQuery, IEnumerable<DishResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<DishResponseDto>>> Handle(
        GetAllDishQuery request, 
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<DishResponseDto>>();

        try
        {
            var dishes = _unitOfWork.Dishes.GetAllQueryable();

            // ✅ Filtro por Nombre (ANTES NO EXISTÍA)
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim().ToLower();
                dishes = dishes.Where(x => x.Name.ToLower().Contains(name));
            }

            // ✅ Filtro por Categoría (ANTES ERA EXACTO)
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                var category = request.Category.Trim().ToLower();
                dishes = dishes.Where(x => x.Category.ToLower().Contains(category));
            }

            // ✅ Filtro por Estados (SE MANTIENE IGUAL)
            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                dishes = dishes.Where(x => stateFilter.Contains(x.State));
            }

            // ✅ Orden por defecto (se respeta tu lógica)
            request.Sort ??= "Category";

            var ordered = dishes
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name);

            var items = await _orderingQuery
                .Ordering(request, ordered)
                .ToListAsync(cancellationToken);

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