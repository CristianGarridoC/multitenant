using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Entity = Product.Domain.Entities;

namespace Product.Application.Common.Data;

public interface IApplicationDbContext : IDbContext
{
    DbSet<Entity.Product> Product { get; }
}