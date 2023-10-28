using FluentResults;
using MediatR;

namespace Product.Application.Product.Commands.Delete;

public record DeleteRequest(int Id): IRequest<Result<Unit>>;