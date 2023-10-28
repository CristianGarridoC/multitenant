using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Data;

namespace Product.Application.Product.Queries.GetAll;

public class GetAllCommandHandler : IRequestHandler<GetAllRequest, Result<GetAllResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GetAllResponse>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext
            .Product
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower().Trim()));
        }

        var result = await query
            .Select(x => new Product(
                x.Id,
                x.Name,
                x.Description,
                x.Duration
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(new GetAllResponse(result));
    }
}