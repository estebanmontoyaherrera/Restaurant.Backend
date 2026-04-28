using Microsoft.EntityFrameworkCore;
using Mapster;
using Ordering.Application.Dtos.Orders;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Queries.GetByIdQuery;

public class GetOrderByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetOrderByIdQuery, OrderByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<OrderByIdResponseDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<OrderByIdResponseDto>();

        try
        {
            var data = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

            if (data is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            var total = await _unitOfWork.OrderDetails.GetAllQueryable()
                .Where(x => x.OrderId == data.Id)
                .SumAsync(x => (decimal?)(x.Quantity * x.UnitPrice), cancellationToken) ?? 0m;

            response.IsSuccess = true;
            response.Data = data.Adapt<OrderByIdResponseDto>() with { Total = total };
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
