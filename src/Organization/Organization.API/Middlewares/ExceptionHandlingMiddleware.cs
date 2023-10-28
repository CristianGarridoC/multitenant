using Newtonsoft.Json;
using Organization.Application.Common;
using Organization.Application.Common.Exceptions;
using Shared.DTO.Common;

namespace Organization.API.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await ExceptionHandler(context, e);
        }
    }
    
    private static async Task ExceptionHandler(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        var response = new ErrorResponse
        {
            Status = statusCode,
            Message = exception switch
            {
                ValidationException validationException => validationException.Message,
                _ => Constants.ErrorMessages.UnhandledError
            },
            Details = exception switch
            {
                ValidationException validationException => validationException!.Errors,
                _ => new Dictionary<string, string[]>()
            }
        };
        
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}