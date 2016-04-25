using Newtonsoft.Json;

namespace SeaMist.Model
{
    public abstract class OptimizeRequestBase : IRequest
    {
        [JsonProperty("lossy")]
        public bool Lossy { get; set; }

        [JsonProperty("webp")]
        public bool WebP { get; set; }

        [JsonProperty("convert")]
        public ConvertImage ConvertImage { get; set; }

        [JsonProperty("resize")]
        public ResizeImage ResizeImage { get; set; }

        [JsonProperty("preserve_meta")]
        public PreserveMeta[] PreserveMeta { get; set; }

        [JsonProperty("auth")]
        public Authentication Authentication { get; set; }

        [JsonProperty("dev")]
        public bool Dev { get; set; }
    }
}