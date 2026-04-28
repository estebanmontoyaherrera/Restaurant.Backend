using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Commands.ToggleAvailabilityCommand;

public class ToggleDishAvailabilityHandler(IUnitOfWork unitOfWork) : ICommandHandler<ToggleDishAvailabilityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(ToggleDishAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var dish = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);
            if (dish is null)
                throw new Exception("Plato no encontrado.");

            dish.IsAvailable = !dish.IsAvailable;
            _unitOfWork.Dishes.UpdateAsync(dish);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "La disponibilidad del plato se ha cambiado correctamente.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
