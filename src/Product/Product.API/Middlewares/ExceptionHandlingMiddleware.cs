using Newtonsoft.Json;
using Npgsql;
using Product.Application.Common;
using Product.Application.Common.Exceptions;
using Shared.DTO.Common;

namespace Product.API.Middlewares;

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
        (int Status, string Message) dbExceptions = DbExceptions(exception);
        
        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            PostgresException => dbExceptions.Status,
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
                PostgresException => dbExceptions.Message,
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

    private static (int status, string message) DbExceptions(Exception exception)
    {
        if (exception.Message.Contains("3D000"))
        {
            return (status: 400, message: "We could not found a product database for the provided tenant");
        }

        return (status: 500, message: Constants.ErrorMessages.UnhandledError);
    }
}