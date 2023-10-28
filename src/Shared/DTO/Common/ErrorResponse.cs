using Newtonsoft.Json;

namespace Shared.DTO.Common;

public class ErrorResponse
{
    [JsonProperty("message")]
    public string Message { get; init; }
    
    [JsonProperty("status")]
    public int Status { get; init; }
    
    [JsonProperty("details")]
    public IReadOnlyDictionary<string, string[]> Details { get; init; }
}