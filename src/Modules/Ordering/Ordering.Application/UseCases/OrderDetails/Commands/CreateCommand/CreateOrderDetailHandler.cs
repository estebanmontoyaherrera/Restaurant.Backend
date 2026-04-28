using Ordering.Application.Interfaces.Services;
using Ordering.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.OrderDetails.Commands.CreateCommand;

public class CreateOrderDetailHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateOrderDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (request.Quantity <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new Exception("Orden no encontrada.");

            if (order.Status != "Abierto")
                throw new Exception("Los detalles del pedido solo se pueden agregar mientras el estado del pedido sea Abierto.");

            var dish = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);
            if (dish is null)
                throw new Exception("Plato no encontrado.");

            if (!dish.IsAvailable)
                throw new Exception("El plato no está disponible.");

            var existingDetail = _unitOfWork.OrderDetails.GetAllQueryable()
                .FirstOrDefault(x => x.OrderId == request.OrderId && x.DishId == request.DishId);

            if (existingDetail is not null)
            {
                existingDetail.Quantity += request.Quantity;
                existingDetail.Notes = request.Notes;
                _unitOfWork.OrderDetails.UpdateAsync(existingDetail);
            }
            else
            {
                var detail = new OrderDetail
                {
                    OrderId = request.OrderId,
                    DishId = request.DishId,
                    Quantity = request.Quantity,
                    UnitPrice = dish.Price,
                    Notes = request.Notes,
                    State = "1"
                };

                await _unitOfWork.OrderDetails.CreateAsync(detail);
            }

            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;
            response.Message = "Los detalles del pedido se guardaron correctamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
