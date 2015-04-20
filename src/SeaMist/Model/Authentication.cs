using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class Authentication
    {
        internal Authentication()
        {
        }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("api_secret")]
        public string ApiSecret { get; set; }
    }
}