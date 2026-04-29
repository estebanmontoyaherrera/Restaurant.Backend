using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Commands.AdvanceStatusCommand;

public class AdvanceOrderStatusHandler(IUnitOfWork unitOfWork) : ICommandHandler<AdvanceOrderStatusCommand, bool>
{
    private static readonly HashSet<string> ValidStatuses = ["Abierto", "En Preparación", "Listo", "Entregado", "Cerrado"];
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(AdvanceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new Exception("Orden no encontrada.");

            if (!ValidStatuses.Contains(order.Status))
                throw new Exception("La orden tiene un estado inválido.");

            if (order.Status == "Abierto")
            {
                // Se agrega la validación de x.State == 1 para platos activos
                var hasItems = _unitOfWork.OrderDetails.GetAllQueryable()
                    .Any(x => x.OrderId == request.OrderId && x.State == "1");

                if (!hasItems)
                    throw new Exception("El pedido debe contener al menos un artículo antes de pasar a la fase de preparación.");

                order.Status = "En Preparación";
            }
            else if (order.Status == "En Preparación")
                order.Status = "Listo";
            else if (order.Status == "Listo")
                order.Status = "Entregado";
            else if (order.Status == "Entregado")
                order.Status = "Cerrado";
            else
                throw new Exception("El estado del pedido no se puede avanzar.");

            _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Data = true;
            response.Message = "El estado del pedido se ha avanzado correctamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}