using Newtonsoft.Json;

namespace Shared.DTO.Common;

public class SuccessfulResponse<TResult>
{
    [JsonProperty("statusText")]
    public string StatusText { get; init; }
    
    [JsonProperty("status")]
    public int Status { get; init; }
    
    [JsonProperty("data")]
    public TResult Data { get; init; }
}