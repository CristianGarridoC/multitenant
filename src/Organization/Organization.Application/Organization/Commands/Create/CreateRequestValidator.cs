using FluentValidation;

namespace Organization.Application.Organization.Commands.Create;

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("The name field is required")
            .MaximumLength(50)
            .WithMessage("The maximum length is 50 characters");
    }
}