namespace Shared.Traceability;

public interface ICorrelationProvider
{
    string Get();
    void Set(string correlationId);
}