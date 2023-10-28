using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Common;
using Shared.DTO.Common;

namespace Product.API.Utils;

public static class ToOkMapper
{
    public static IActionResult ToOk<TResult>(this Result<TResult> result,  int successfulStatusCode = StatusCodes.Status200OK) where TResult : notnull
    {
        if (!result.IsFailed)
        {
            return successfulStatusCode == StatusCodes.Status200OK ? new JsonResult(new SuccessfulResponse<TResult>
            {
                Status = StatusCodes.Status200OK,
                StatusText = "Successful request",
                Data = result.Value
            }) : new NoContentResult();
        }

        var error = (
            from errors in result.Errors
            where errors.GetType() == typeof(ErrorMessage)
            select errors as ErrorMessage
        ).FirstOrDefault();

        const string contentType = "application/json";

        return error?.StatusCode switch
        {
            HttpStatusCode.BadRequest => new JsonResult(BuildErrorResponse(
                error,
                StatusCodes.Status400BadRequest,
                Constants.ErrorMessages.BadRequestError
            ))
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            HttpStatusCode.NotFound => new JsonResult(BuildErrorResponse(
                error,
                StatusCodes.Status404NotFound,
                Constants.ErrorMessages.NotFoundError
            ))
            {
                StatusCode = StatusCodes.Status404NotFound
            },
            _ => new JsonResult(BuildErrorResponse(
                error,
                StatusCodes.Status500InternalServerError,
                Constants.ErrorMessages.UnhandledError
            ))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

    private static object BuildErrorResponse(ErrorMessage? error, int status, string message)
    {
        return new ErrorResponse
        {
            Status = status,
            Message = string.IsNullOrWhiteSpace(error?.Message) ? message : error.Message,
            Details = new Dictionary<string, string[]>()
        };
    }
}