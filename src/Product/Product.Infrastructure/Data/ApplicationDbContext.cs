using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.Application.Common.Data;
using Product.Application.Common.Exceptions;
using Shared.Data;
using Entity = Product.Domain.Entities;

namespace Product.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ConnectionOptions _connectionOptions;
    private readonly ILogger<ApplicationDbContext> _logger;
    
    public ApplicationDbContext(
        IOptions<ConnectionOptions> connectionOptions,
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor contextAccessor,
        ILogger<ApplicationDbContext> logger) : base(options)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
        _connectionOptions = connectionOptions is not null ? connectionOptions.Value : new ConnectionOptions();
    }
    
    public DbSet<Entity.Product> Product => Set<Entity.Product>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        bool filteringAssemblies(Type type) => (type.GetInterface(typeof(IEntityTypeConfiguration<>).Name) != null
                                                && type is { IsSealed: true, IsClass: true, IsNotPublic: true });
        modelBuilder.HasDefaultSchema("multi_tenant");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), filteringAssemblies);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        if (_contextAccessor.HttpContext == null ||
            !_contextAccessor.HttpContext.Items.TryGetValue("tenant", out var tenant))
        {
            throw new InvalidTenant("Unable to configure tenant for database connection");
        }
        
        _logger.LogInformation("Setting tenant \"{Tenant}\" in connection", tenant);
        optionsBuilder.UseNpgsql(_connectionOptions.Default.Replace("#tenant#",
            tenant!.ToString()));
    }
}