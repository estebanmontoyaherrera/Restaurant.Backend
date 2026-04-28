using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Orders.Commands.DeleteCommand;

public class DeleteOrderCommand : ICommand<bool>
{
    public int OrderId { get; set; }
}
