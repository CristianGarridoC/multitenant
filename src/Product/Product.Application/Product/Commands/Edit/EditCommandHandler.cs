using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Edit;

public class EditCommandHandler : IRequestHandler<EditRequest, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;

    public EditCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Unit>> Handle(EditRequest request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Fail(new ErrorMessage("We could not found the product"));
        }
        product.Description = request.Description;
        product.Name = request.Name.Trim();
        product.Duration = request.Duration;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}