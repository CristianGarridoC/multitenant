using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Organization.Application.Common.Data;

namespace Organization.Application.Organization.Queries.GetAll;

public class GetAllCommandHandler : IRequestHandler<GetAllRequest, Result<GetAllResult>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GetAllResult>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext
            .Organization
            .Include(x => x.UserOrganization)
            .ThenInclude(x => x.User)
            .AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower().Trim()));
        }

        var result = await query
            .Select(x => new Organization(
                x.Id,
                x.Name,
                x.SlugTenant,
                x.UserOrganization.Select(user => new User(user.UserId, user.User.Email))
            ))
            .ToListAsync(cancellationToken);

        return Result.Ok(new GetAllResult(result));
    }
}