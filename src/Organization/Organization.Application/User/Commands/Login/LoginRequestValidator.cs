using FluentValidation;

namespace Organization.Application.User.Commands.Login;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("The email field is required")
            .EmailAddress();
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("The password field is required");
    }
}