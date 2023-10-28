namespace Product.API.Middlewares;

public class TenantHandlingMiddleware : IMiddleware
{
    private readonly ILogger<TenantHandlingMiddleware> _logger;

    public TenantHandlingMiddleware(ILogger<TenantHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        HandleTenant(context);
        await next(context);
    }

    private void HandleTenant(HttpContext context)
    {
        if (context.Request.RouteValues.TryGetValue("slugTenant", out var tenant))
        {
            context.Items.Add("tenant", tenant);
        }
    }
}