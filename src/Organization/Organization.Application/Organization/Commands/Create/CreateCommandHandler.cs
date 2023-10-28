using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Organization.Application.Common;
using Organization.Application.Common.Data;
using Slugify;

namespace Organization.Application.Organization.Commands.Create;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateRequest, Result<CreateResult>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CreateOrganizationCommandHandler> _logger;

    public CreateOrganizationCommandHandler(IApplicationDbContext dbContext,
        IHttpClientFactory httpClientFactory, ILogger<CreateOrganizationCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Result<CreateResult>> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var alreadyExistsOrg = await _dbContext
                .Organization
                .FirstOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower().Trim(), cancellationToken);

            if (alreadyExistsOrg is not null)
            {
                return Result.Fail(new ErrorMessage(Constants.Organization.OrganizationAlreadyExists));
            }
        
            var helper = new SlugHelper();
            var slug = helper.GenerateSlug(request.Name.Trim());
            if (string.IsNullOrWhiteSpace(slug))
            {
                return Result.Fail(new ErrorMessage("It seems that the submitted name contains invalid characters resulting in an empty slug"));
            }
            
            var organization = new Domain.Entities.Organization
            {
                Name = request.Name.Trim(),
                SlugTenant = slug
            };
            await _dbContext.Organization.AddAsync(organization, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!await CreateProductDatabase(slug))
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Fail(new ErrorMessage("We could not create the product database for this tenant"));
            }
            
            await transaction.CommitAsync(cancellationToken);
            return Result.Ok(new CreateResult(organization.Id));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<bool> CreateProductDatabase(string slugTenant)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Product.API");
            var result = await client.GetAsync($"api/v1/tenant/{slugTenant}/association");
            return (int)result.StatusCode == 204;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error when trying to create the product's database for tenant {SlugTenant}", slugTenant);
            throw;
        }
    }
}