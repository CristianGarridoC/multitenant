using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Common.Data;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.Common.Interfaces.Services;
using Organization.Infrastructure.Authentication;
using Organization.Infrastructure.Data;
using Organization.Infrastructure.Services;
using Serilog;
using Shared.Traceability;

namespace Organization.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("default"))
        );
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<ICorrelationProvider, CorrelationProvider>();
        return services;
    }
    
    public static ConfigureHostBuilder AddSerilogConfiguration(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
        );
        return host;
    }
}