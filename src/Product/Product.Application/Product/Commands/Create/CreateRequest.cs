using FluentResults;
using MediatR;

namespace Product.Application.Product.Commands.Create;

public record CreateRequest(string Name, string Description, int Duration) : IRequest<Result<CreateResponse>>;