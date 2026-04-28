using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.OrderDetails;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetAllQuery;

public class GetAllOrderDetailHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery) : IQueryHandler<GetAllOrderDetailQuery, IEnumerable<OrderDetailResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<OrderDetailResponseDto>>> Handle(GetAllOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<OrderDetailResponseDto>>();

        try
        {
            var data = _unitOfWork.OrderDetails.GetAllQueryable();

            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                data = data.Where(x => stateFilter.Contains(x.State));
            }

            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                data = data.Where(x => x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                    x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1));
            }

            request.Sort ??= "Id";

            var items = await _orderingQuery.Ordering(request, data).ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await data.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<OrderDetailResponseDto>>();
            response.Message = "Query successful.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
