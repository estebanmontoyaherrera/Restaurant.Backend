using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Dishes.Commands.UpdateCommand;

public class UpdateDishCommand : ICommand<bool>
{
    public int DishId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public string? State { get; set; }
}
