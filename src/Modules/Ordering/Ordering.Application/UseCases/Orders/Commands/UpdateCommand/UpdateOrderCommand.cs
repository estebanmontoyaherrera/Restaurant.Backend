using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Orders.Commands.UpdateCommand;

public class UpdateOrderCommand : ICommand<bool>
{
    public int OrderId { get; set; }
    public int TableNumber { get; set; }
    public string WaiterName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? State { get; set; }
}
