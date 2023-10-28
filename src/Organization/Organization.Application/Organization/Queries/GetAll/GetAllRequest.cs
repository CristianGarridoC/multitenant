using FluentResults;
using MediatR;

namespace Organization.Application.Organization.Queries.GetAll;

public record GetAllRequest(string Name) : IRequest<Result<GetAllResult>>;