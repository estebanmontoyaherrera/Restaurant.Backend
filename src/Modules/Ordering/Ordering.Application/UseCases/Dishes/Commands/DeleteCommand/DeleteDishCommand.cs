using SharedKernel.Abstractions.Messaging;

namespace Ordering.Application.UseCases.Dishes.Commands.DeleteCommand;

public class DeleteDishCommand : ICommand<bool>
{
    public int DishId { get; set; }
}
