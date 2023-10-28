using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Common.Data;
using Product.Infrastructure.Data;
using Serilog;
using Shared.Traceability;

namespace Product.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        if (!IsDesignTime())
        {
            services.AddDbContext<ApplicationDbContext>();
        }
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<ICorrelationProvider, CorrelationProvider>();
        return services;
    }
    
    private static bool IsDesignTime()
    {
        var args = Environment.GetCommandLineArgs();
        if (args.Length < 1)
        {
            return false;
        }

        var arg = args[0];
        return Path.GetFileName(arg) == "ef.dll";
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