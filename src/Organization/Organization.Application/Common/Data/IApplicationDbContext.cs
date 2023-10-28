using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Entity = Organization.Domain.Entities;

namespace Organization.Application.Common.Data;

public interface IApplicationDbContext : IDbContext
{
    DbSet<Entity.Organization> Organization { get; }
    DbSet<Entity.User> User { get; }
    DbSet<Entity.UserOrganization> UserOrganization { get; }
}