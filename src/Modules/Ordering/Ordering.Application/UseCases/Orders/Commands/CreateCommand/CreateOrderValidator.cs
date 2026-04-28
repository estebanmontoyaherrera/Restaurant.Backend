using FluentValidation;

namespace Ordering.Application.UseCases.Orders.Commands.CreateCommand;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.TableNumber).InclusiveBetween(1, 50);
        RuleFor(x => x.WaiterName).NotEmpty();
    }
}
