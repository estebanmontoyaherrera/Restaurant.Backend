using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Orders.Commands.CreateCommand;

public class CreateOrderCommand : ICommand<bool>
{
    public int TableNumber { get; set; }
    public string WaiterName { get; set; } = null!;
}
