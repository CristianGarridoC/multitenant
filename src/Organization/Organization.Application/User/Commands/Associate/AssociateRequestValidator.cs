using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Organization.Application.Common.Data;

namespace Organization.Application.User.Commands.Associate;

public class AssociateRequestValidator : AbstractValidator<AssociateRequest>
{
    public AssociateRequestValidator(IApplicationDbContext dbContext)
    {
        RuleFor(p => p.UserId)
            .NotEmpty();
        
        RuleFor(p => p.Tenants)
            .NotNull()
            .NotEmpty()
            .Must(list => list.Count() < 5)
            .WithMessage("The tenants must contain fewer than 5 items")
            .Custom((tenants, context) =>
            {
                var _tenants = tenants.Distinct();
                
                var validIds = dbContext
                    .Organization
                    .Where(x => _tenants.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();
                
                var invalidIdsLst = _tenants
                    .Except(validIds)
                    .ToHashSet();

                if (invalidIdsLst.Any())
                {
                    context.AddFailure($"The following tenant ids: {string.Join(", ", invalidIdsLst)} are invalid");
                }
            });
        
        RuleFor(x => new { x.UserId, x.Tenants })
            .NotNull()
            .OverridePropertyName("Tenants")
            .Custom((properties, context) =>
            {
                var _tenants = properties.Tenants.Distinct();

                var currentTenantIds = dbContext
                    .User
                    .Include(x => x.UserOrganization)
                    .Where(x => x.Id == properties.UserId)
                    .SelectMany(x => x.UserOrganization)
                    .Select(x => x.OrganizationId)
                    .ToList();
                
                var existingIds = currentTenantIds.Intersect(_tenants);
                if (existingIds.Any())
                {
                    context.AddFailure(
                        $"The following tenant ids: {string.Join(", ", existingIds)} have already been assigned to the user");
                }
            });
    }   
}