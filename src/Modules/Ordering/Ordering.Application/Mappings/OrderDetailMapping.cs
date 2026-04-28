using Mapster;
using Ordering.Application.Dtos.OrderDetails;
using Ordering.Application.UseCases.OrderDetails.Commands.CreateCommand;
using Ordering.Application.UseCases.OrderDetails.Commands.UpdateCommand;
using Ordering.Domain.Entities;
using SharedKernel.Dtos.Commons;

namespace Ordering.Application.Mappings;

public class OrderDetailMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderDetail, OrderDetailResponseDto>()
          .Map(dest => dest.OrderDetailId, src => src.Id)
          .Map(dest => dest.Subtotal, src => src.Quantity * src.UnitPrice)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Enabled" : "Disabled")
          .TwoWays();

        config.NewConfig<OrderDetail, OrderDetailByIdResponseDto>()
          .Map(dest => dest.OrderDetailId, src => src.Id)
          .Map(dest => dest.Subtotal, src => src.Quantity * src.UnitPrice)
          .TwoWays();

        config.NewConfig<CreateOrderDetailCommand, OrderDetail>();
        config.NewConfig<UpdateOrderDetailCommand, OrderDetail>();

        config.NewConfig<OrderDetail, SelectResponseDto>()
          .Map(dest => dest.Code, src => src.Id)
          .Map(dest => dest.Description, src => $"Order {src.OrderId} - Dish {src.DishId}")
          .TwoWays();
    }
}
