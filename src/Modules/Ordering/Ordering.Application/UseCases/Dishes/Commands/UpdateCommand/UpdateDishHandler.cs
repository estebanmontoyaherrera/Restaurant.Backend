using Ordering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Commands.UpdateCommand;

public class UpdateDishHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateDishCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Se requiere el nombre del plato.");

            if (request.Price <= 0)
                throw new Exception("El precio del plato debe ser mayor que cero.");

            var exists = _unitOfWork.Dishes.GetAllQueryable()
                .Any(x => x.Id != request.DishId && x.Name.ToLower() == request.Name.ToLower());

            if (exists)
                throw new Exception("Dish name already exists.");

            var dish = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);
            if (dish is null)
                throw new Exception("Plato no encontrado.");

            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.IsAvailable = request.IsAvailable;
            dish.State = request.State;

            _unitOfWork.Dishes.UpdateAsync(dish);
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
