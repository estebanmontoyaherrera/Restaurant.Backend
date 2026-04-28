using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Commands.UpdateCommand;

public class UpdateOrderDetailHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateOrderDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (request.Quantity <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            var detail = await _unitOfWork.OrderDetails.GetByIdAsync(request.OrderDetailId);
            if (detail is null)
                throw new Exception("No se encontraron los detalles del pedido.");

            if (detail.OrderId != request.OrderId)
                throw new Exception("El detalle del pedido no pertenece a la orden especificada.");

            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new Exception("Orden no encontrada.");

            if (order.Status != "Abierto")
                throw new Exception("Los detalles del pedido solo se pueden modificar mientras el estado del pedido sea Abierto.");

            var duplicateDetail = _unitOfWork.OrderDetails.GetAllQueryable()
                .Any(x => x.OrderId == request.OrderId &&
                          x.DishId == request.DishId &&
                          x.Id != request.OrderDetailId);

            if (duplicateDetail)
                throw new Exception("Ya existe un detalle para este plato en la orden.");

            detail.OrderId = request.OrderId;
            detail.DishId = request.DishId;
            detail.Quantity = request.Quantity;
            detail.UnitPrice = request.UnitPrice;
            detail.Notes = request.Notes;
            detail.State = request.State;

            _unitOfWork.OrderDetails.UpdateAsync(detail);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro actualizado exitosamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
