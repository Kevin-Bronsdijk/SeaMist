using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeResult
    {
        internal OptimizeResult()
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}