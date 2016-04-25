using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class SubAccount
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("api_secret")]
        public string ApiSecret { get; set; }

        [JsonProperty("current_usage")]
        public double CurrentUsage { get; set; }
    }
}