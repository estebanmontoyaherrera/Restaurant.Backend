using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.OrderDetails.Commands.UpdateCommand;

public class UpdateOrderDetailCommand : ICommand<bool>
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
    public string? State { get; set; }
}
