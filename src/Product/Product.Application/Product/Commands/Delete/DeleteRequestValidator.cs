using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Delete;

public class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator(IApplicationDbContext dbContext)
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
            .WithMessage("We could not found the product with the given id");;
    }
}