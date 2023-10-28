namespace Shared.Traceability;

public record HeaderNameOptions
{
    public string CorrelationHeader { get; init; }
    public string AuthorizationHeader { get; init; }
}