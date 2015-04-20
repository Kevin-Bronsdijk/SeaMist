using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public abstract class OptimizeRequestBase : IOptimizeRequest
    {
        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("lossy")]
        public bool Lossy { get; set; }

        [JsonProperty("webp")]
        public bool WebP { get; set; }

        [JsonProperty("convert")]
        public ConvertImage ConvertImage { get; set; }

        [JsonProperty("resize")]
        public ResizeImage ResizeImage { get; set; }

        [JsonProperty("auth")]
        public Authentication Authentication { get; set; }
    }
}