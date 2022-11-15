using Newtonsoft.Json;

namespace HelsiTaskManager.Repository;
public class BaseResponse
{
    public bool IsSuccess { get; set; }
    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }
}
