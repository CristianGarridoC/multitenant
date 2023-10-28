using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Traceability;

namespace Shared.OptionsSetup;

public class HeaderNameOptionsSetup : IConfigureOptions<HeaderNameOptions>
{
    private readonly IConfiguration _configuration;

    public HeaderNameOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(HeaderNameOptions options)
    {
        _configuration.GetRequiredSection("HeaderName").Bind(options);
    }
}