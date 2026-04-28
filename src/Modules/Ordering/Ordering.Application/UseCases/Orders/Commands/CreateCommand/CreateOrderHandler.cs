using Mapster;
using Ordering.Application.Interfaces.Services;
using Ordering.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Commands.CreateCommand;

public class CreateOrderHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (request.TableNumber < 1 || request.TableNumber > 50)
                throw new Exception("El número de mesa debe estar entre 1 y 50.");

            if (string.IsNullOrWhiteSpace(request.WaiterName))
                throw new Exception("Se requiere el nombre del camarero.");

            var existsOpenOrder = _unitOfWork.Orders.GetAllQueryable()
                .Any(x => x.TableNumber == request.TableNumber && x.Status == "Abierto");

            if (existsOpenOrder)
                throw new Exception("Ya existe una orden abierta para esta mesa.");

            var order = request.Adapt<Order>();
            order.Status = "Abierto";
            order.State = "1";

            await _unitOfWork.Orders.CreateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro creado exitosamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
