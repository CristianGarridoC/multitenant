using FluentResults;
using MediatR;

namespace Product.Application.Product.Queries.GetAll;

public record GetAllRequest(string? Name) : IRequest<Result<GetAllResponse>>;