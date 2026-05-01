using Mapster;
using Ordering.Application.Dtos.Dishes;
using Ordering.Application.UseCases.Dishes.Commands.CreateCommand;
using Ordering.Application.UseCases.Dishes.Commands.UpdateCommand;
using Ordering.Domain.Entities;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.Mappings;

public class DishMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Dish, DishResponseDto>()
          .Map(dest => dest.DishId, src => src.Id)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Activo" : "Inactivo")
          .TwoWays();

        config.NewConfig<Dish, DishByIdResponseDto>()
          .Map(dest => dest.DishId, src => src.Id)
          .TwoWays();

        config.NewConfig<CreateDishCommand, Dish>();
        config.NewConfig<UpdateDishCommand, Dish>();

        config.NewConfig<Dish, SelectResponseDto>()
          .Map(dest => dest.Code, src => src.Id)
          .Map(dest => dest.Description, src => src.Name)
          .TwoWays();
    }
}
