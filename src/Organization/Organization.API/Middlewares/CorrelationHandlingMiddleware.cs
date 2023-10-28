using Microsoft.Extensions.Options;
using Shared.Traceability;

namespace Organization.API.Middlewares;

public class CorrelationHandlingMiddleware : IMiddleware
{
    private readonly ICorrelationProvider _correlationProvider;
    private readonly HeaderNameOptions _headerName;

    public CorrelationHandlingMiddleware(ICorrelationProvider correlationProvider,
        IOptions<HeaderNameOptions> headerName)
    {
        _correlationProvider = correlationProvider;
        _headerName = headerName.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        HandleCorrelation(context);
        await next(context);
    }

    private void HandleCorrelation(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_headerName.CorrelationHeader, out var correlationId))
        {
            _correlationProvider.Set(correlationId);
        }
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(_headerName.CorrelationHeader))
            {
                context.Response.Headers.Add(_headerName.CorrelationHeader, _correlationProvider.Get());
            }
            return Task.CompletedTask;
        });
    }
}