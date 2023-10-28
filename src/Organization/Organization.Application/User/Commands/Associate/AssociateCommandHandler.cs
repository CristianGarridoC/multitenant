using System.Net;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Organization.Application.Common;
using Organization.Application.Common.Data;
using Organization.Domain.Entities;

namespace Organization.Application.User.Commands.Associate;

public class AssociateCommandHandler : IRequestHandler<AssociateRequest, Result<Unit>>
{
    private readonly IApplicationDbContext _dbContext;

    public AssociateCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Unit>> Handle(AssociateRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Fail(new ErrorMessage(Constants.User.UserNotFound, HttpStatusCode.NotFound));
        }

        var tenants = request.Tenants.Distinct();
        foreach (var tenant in tenants)
        {
            await _dbContext.UserOrganization.AddAsync(
                new UserOrganization { UserId = user.Id, OrganizationId = tenant },
                cancellationToken);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}