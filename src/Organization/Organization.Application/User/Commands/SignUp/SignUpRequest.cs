using FluentResults;
using MediatR;
using Organization.Application.User.Commands.Common;

namespace Organization.Application.User.Commands.SignUp;

public record SignUpRequest(string Email, string Password) : IRequest<Result<GenericResponse>>;