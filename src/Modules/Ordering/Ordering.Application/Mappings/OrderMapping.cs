using Mapster;
using Ordering.Application.Dtos.Orders;
using Ordering.Application.UseCases.Orders.Commands.CreateCommand;
using Ordering.Application.UseCases.Orders.Commands.UpdateCommand;
using Ordering.Domain.Entities;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.Mappings;

public class OrderMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderResponseDto>()
          .Map(dest => dest.OrderId, src => src.Id)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Activo" : "Inactivo")
          .TwoWays();

        config.NewConfig<Order, OrderByIdResponseDto>()
          .Map(dest => dest.OrderId, src => src.Id)
          .TwoWays();

        config.NewConfig<CreateOrderCommand, Order>();
        config.NewConfig<UpdateOrderCommand, Order>();

        config.NewConfig<Order, SelectResponseDto>()
          .Map(dest => dest.Code, src => src.Id)
          .Map(dest => dest.Description, src => $"Table {src.TableNumber}")
          .TwoWays();
    }
}
