using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Commands.DeleteCommand;

public class DeleteDishHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteDishCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var hasDetails = _unitOfWork.OrderDetails.GetAllQueryable().Any(x => x.DishId == request.DishId);
            if (hasDetails)
                throw new Exception("El plato no se puede eliminar porque se utiliza en los detalles del pedido.");

            await _unitOfWork.Dishes.DeleteAsync(request.DishId);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro eliminado exitosamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
