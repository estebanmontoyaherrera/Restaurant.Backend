using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Dishes.Commands.CreateCommand;

public class CreateDishCommand : ICommand<bool>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
}
