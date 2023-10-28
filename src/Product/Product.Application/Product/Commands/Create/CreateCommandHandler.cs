using FluentResults;
using MediatR;
using Product.Application.Common.Data;

namespace Product.Application.Product.Commands.Create;

public class CreateCommandHandler : IRequestHandler<CreateRequest, Result<CreateResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CreateResponse>> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = new()
        {
            Name = request.Name,
            Description = request.Description,
            Duration = request.Duration
        };

        await _dbContext.Product.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(new CreateResponse(product.Id));
    }
}