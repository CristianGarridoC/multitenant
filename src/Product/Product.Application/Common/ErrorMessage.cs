using System.Net;
using FluentResults;

namespace Product.Application.Common;

public class ErrorMessage : Error
{
    public HttpStatusCode StatusCode { get; }
    public ErrorMessage(string message = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message)
    {
        StatusCode = statusCode;
    }
}