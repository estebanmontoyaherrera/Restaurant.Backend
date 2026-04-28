using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.OrderDetails.Commands.DeleteCommand;

public class DeleteOrderDetailCommand : ICommand<bool>
{
    public int OrderId { get; set; }
    public int OrderDetailId { get; set; }
}
