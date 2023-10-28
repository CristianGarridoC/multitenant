using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Data;

namespace Shared.OptionsSetup;

public class ConnectionOptionsSetup : IConfigureOptions<ConnectionOptions>
{
    private readonly IConfiguration _configuration;
    
    public ConnectionOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ConnectionOptions options)
    {
        _configuration.GetSection("ConnectionStrings").Bind(options);
    }
}