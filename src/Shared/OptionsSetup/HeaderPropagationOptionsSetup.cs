using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.Options;
using Shared.Traceability;

namespace Shared.OptionsSetup;

public class HeaderPropagationOptionsSetup : IPostConfigureOptions<HeaderPropagationOptions>
{
    private readonly HeaderNameOptions _headerNameOptions;

    public HeaderPropagationOptionsSetup(IOptions<HeaderNameOptions> headerNameOptions)
    {
        _headerNameOptions = headerNameOptions.Value;
    }

    public void PostConfigure(string? name, HeaderPropagationOptions options)
    {
        options.Headers.Add(_headerNameOptions.CorrelationHeader);
        if (!string.IsNullOrWhiteSpace(_headerNameOptions.AuthorizationHeader))
        {
            options.Headers.Add(_headerNameOptions.AuthorizationHeader);
        }
    }
}