using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos.OrderDetails;
using Ordering.Application.Interfaces.Services;
using Ordering.Application.UseCases.OrderDetails.Queries.GetByIdQuery;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetByOrderIdQuery;

public class GetOrderDetailsByOrderIdHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetOrderDetailsByOrderIdQuery, IEnumerable<OrderDetailResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<IEnumerable<OrderDetailResponseDto>>> Handle(
        GetOrderDetailsByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<OrderDetailResponseDto>>();

        try
        {
            // 1. Validar que la orden exista
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

            if (order is null)
                throw new Exception("Orden no encontrada.");

            // 2. Query FUERTEMENTE TIPADA (evita error de inferencia)
            var query = _unitOfWork.OrderDetails
                .GetAllQueryable()
                .AsQueryable(); // fuerza IQueryable EF

            // 3. Ejecutar consulta
            var details = await query
                .Where(x => x.OrderId == request.OrderId)
                .OrderByDescending(x => x.AuditCreateDate)
                .ToListAsync(cancellationToken);

            // 4. Mapear
            var data = details.Adapt<IEnumerable<OrderDetailResponseDto>>();

            // 5. Response
            response.IsSuccess = true;
            response.Data = data;
            response.TotalRecords = details.Count;
            response.Message = "Items de la orden obtenidos correctamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}