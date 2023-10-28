using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Organization.Application.Common.Data;
using Entity = Organization.Domain.Entities;

namespace Organization.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Entity.Organization> Organization => Set<Entity.Organization>();
    public DbSet<Entity.User> User => Set<Entity.User>();
    public DbSet<Entity.UserOrganization> UserOrganization => Set<Entity.UserOrganization>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        bool filteringAssemblies(Type type) => (type.GetInterface(typeof(IEntityTypeConfiguration<>).Name) != null
                                                && type is { IsSealed: true, IsClass: true, IsNotPublic: true });
        modelBuilder.HasDefaultSchema("multi_tenant");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), filteringAssemblies);
    }
}