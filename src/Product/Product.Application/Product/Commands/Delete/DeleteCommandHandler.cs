using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteRequest, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Unit>> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Fail(new ErrorMessage("We could not found the product"));
        }
        _dbContext.Product.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}