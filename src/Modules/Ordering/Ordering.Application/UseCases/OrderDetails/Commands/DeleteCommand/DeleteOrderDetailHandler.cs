using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Commands.DeleteCommand;

public class DeleteOrderDetailHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteOrderDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var existsOrderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(request.OrderDetailId);

            if (existsOrderDetail is null)
                throw new Exception("No se encontraron los detalles del pedido.");

            if (request.OrderId > 0 && existsOrderDetail.OrderId != request.OrderId)
                throw new Exception("El detalle del pedido no pertenece a la orden especificada.");

            var order = await _unitOfWork.Orders.GetByIdAsync(existsOrderDetail.OrderId);
            if (order is null)
                throw new Exception("Orden no encontrada.");

            if (order.Status != "Abierto")
                throw new Exception("Los detalles del pedido solo se pueden eliminar mientras el estado del pedido sea Abierto.");

            await _unitOfWork.OrderDetails.DeleteAsync(request.OrderDetailId);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro eliminado exitosamente.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
