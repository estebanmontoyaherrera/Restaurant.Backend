using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Orders.Commands.AdvanceStatusCommand;

public class AdvanceOrderStatusCommand : ICommand<bool>
{
    public int OrderId { get; set; }
}
