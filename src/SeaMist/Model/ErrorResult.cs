using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class ErrorResult
    {
        [JsonProperty("message")]
        public string Error { get; set; }
    }
}