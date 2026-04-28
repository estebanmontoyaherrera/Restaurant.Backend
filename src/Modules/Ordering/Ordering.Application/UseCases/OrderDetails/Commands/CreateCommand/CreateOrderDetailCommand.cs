using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.OrderDetails.Commands.CreateCommand;

public class CreateOrderDetailCommand : ICommand<bool>
{
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
