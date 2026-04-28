using FluentValidation;

namespace Ordering.Application.UseCases.Dishes.Commands.CreateCommand;

public class CreateDishValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
    }
}
