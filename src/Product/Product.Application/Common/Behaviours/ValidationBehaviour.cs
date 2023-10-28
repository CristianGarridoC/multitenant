using FluentValidation;
using MediatR;
using ValidationException = Product.Application.Common.Exceptions.ValidationException;

namespace Product.Application.Common.Behaviours;

internal sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators
                .Select(x => x.ValidateAsync(context, cancellationToken))
        );
        var failures = validationResults
            .Where(x => x.Errors.Any())
            .SelectMany(x => x.Errors)
            .ToList();
        if (!failures.Any())
        {
            return await next();
        }

        throw new ValidationException(failures);
    }
}