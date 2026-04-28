using Mapster;
using Ordering.Application.Interfaces.Services;
using Ordering.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Ordering.Application.UseCases.Dishes.Commands.CreateCommand;

public class CreateDishHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateDishCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Se requiere el nombre del plato.");

            if (request.Price <= 0)
                throw new Exception("El precio del plato debe ser mayor que cero.");

            var exists = _unitOfWork.Dishes.GetAllQueryable()
                .Any(x => x.Name.ToLower() == request.Name.ToLower());

            if (exists)
                throw new Exception("El nombre del plato ya existe.");

            var dish = request.Adapt<Dish>();
            dish.IsAvailable = true;
            dish.State = "1";

            await _unitOfWork.Dishes.CreateAsync(dish);
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
