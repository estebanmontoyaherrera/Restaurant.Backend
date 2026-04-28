using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Dishes.Commands.ToggleAvailabilityCommand;

public class ToggleDishAvailabilityCommand : ICommand<bool>
{
    public int DishId { get; set; }
}
