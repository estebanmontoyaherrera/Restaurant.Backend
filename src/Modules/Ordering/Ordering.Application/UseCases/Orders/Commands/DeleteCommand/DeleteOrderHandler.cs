using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Orders.Commands.DeleteCommand;

public class DeleteOrderHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var existsOrder = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

            if (existsOrder is null)
            {
                response.IsSuccess = false;
                response.Message = "El registro no existe en la base de datos.";
                return response;
            }

            await _unitOfWork.Orders.DeleteAsync(request.OrderId);
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
