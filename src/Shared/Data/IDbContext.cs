using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Shared.Data;

public interface IDbContext : IDisposable
{
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}