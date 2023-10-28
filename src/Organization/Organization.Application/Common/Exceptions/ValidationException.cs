using FluentValidation.Results;
using static Organization.Application.Common.Constants;

namespace Organization.Application.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    private ValidationException() : base(ErrorMessages.ValidationError)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(x => x.PropertyName, x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Value = errorMessages.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Value);
    }
}