using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Commands.UpdateCommand;

public class UpdateOrderHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateOrderCommand, bool>
{
    private static readonly HashSet<string> ValidStatuses = ["Abierto", "En Preparación", "Listo", "Entregado", "Cerrado"];
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (request.TableNumber < 1 || request.TableNumber > 50)
                throw new Exception("El número de mesa debe estar entre 1 y 50.");

            if (string.IsNullOrWhiteSpace(request.WaiterName))
                throw new Exception("Se requiere el nombre del camarero.");

            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new Exception("Orden no encontrada.");

            if (!ValidStatuses.Contains(order.Status))
                throw new Exception("La orden tiene un estado inválido.");

            if (!string.IsNullOrWhiteSpace(request.Status) && request.Status != order.Status)
                throw new Exception("El estado solo se puede cambiar con el flujo de avance de estado.");

            order.TableNumber = request.TableNumber;
            order.WaiterName = request.WaiterName;
            order.State = request.State;

            _unitOfWork.Orders.UpdateAsync(order);
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
