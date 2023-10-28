using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Edit;

public class EditRequestValidator : AbstractValidator<EditRequest>
{
    public EditRequestValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await dbContext
                    .Product
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                return exists is not null;
            })
            .WithMessage("We could not found the product with the given id");
        
        RuleFor(x => new { x.Name, x.Id })
            .NotNull()
            .OverridePropertyName("Name")
            .MustAsync(async (property, cancellationToken) =>
            {
                var exists = await dbContext
                    .Product
                    .FirstOrDefaultAsync(x => property.Id != x.Id &&
                                              x.Name.ToLower().Trim() == property.Name.ToLower().Trim(),
                        cancellationToken);

                return exists is null;
            })
            .WithMessage(Constants.Product.ProductAlreadyExists);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}