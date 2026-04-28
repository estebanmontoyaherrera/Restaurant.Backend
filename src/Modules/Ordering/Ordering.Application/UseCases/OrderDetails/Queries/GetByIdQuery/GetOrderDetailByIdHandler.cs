using Mapster;
using Ordering.Application.Dtos.OrderDetails;
using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Queries.GetByIdQuery;

public class GetOrderDetailByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetOrderDetailByIdQuery, OrderDetailByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<OrderDetailByIdResponseDto>> Handle(GetOrderDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<OrderDetailByIdResponseDto>();

        try
        {
            var data = await _unitOfWork.OrderDetails.GetByIdAsync(request.OrderDetailId);

            if (data is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = data.Adapt<OrderDetailByIdResponseDto>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
