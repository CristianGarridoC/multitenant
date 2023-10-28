using FluentResults;
using MediatR;
using Organization.Application.User.Commands.Common;

namespace Organization.Application.User.Commands.Login;

public record LoginRequest(string Email, string Password) : IRequest<Result<GenericResponse>>;