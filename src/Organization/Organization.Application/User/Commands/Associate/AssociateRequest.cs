using FluentResults;
using MediatR;

namespace Organization.Application.User.Commands.Associate;

public record AssociateRequest(int UserId, IEnumerable<int> Tenants) : IRequest<Result<Unit>>;