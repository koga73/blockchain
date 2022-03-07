using Newtonsoft.Json;

namespace Q.API.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }
    }
}
