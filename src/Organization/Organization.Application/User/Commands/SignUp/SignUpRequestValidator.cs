using FluentValidation;

namespace Organization.Application.User.Commands.SignUp;

internal sealed class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("The password field is required");
        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("The email field is required")
            .EmailAddress();
    }
}