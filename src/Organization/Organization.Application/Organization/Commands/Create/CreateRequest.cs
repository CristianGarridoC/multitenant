using FluentResults;
using MediatR;

namespace Organization.Application.Organization.Commands.Create;

public record CreateRequest(string Name) : IRequest<Result<CreateResult>>;