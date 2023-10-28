using FluentResults;
using MediatR;

namespace Product.Application.Product.Commands.Edit;

public record EditRequest(int Id, string Name, string Description, int Duration) : IRequest<Result<Unit>>;