using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Create;

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(async (name, cancellationToken) =>
            {
                var exists = await dbContext
                    .Product
                    .FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim(), cancellationToken);

                return exists is null;
            })
            .WithMessage(Constants.Product.ProductAlreadyExists);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}