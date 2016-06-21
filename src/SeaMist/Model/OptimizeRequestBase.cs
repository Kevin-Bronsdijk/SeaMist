using Newtonsoft.Json;

namespace SeaMist.Model
{
    public abstract class OptimizeRequestBase : IRequest
    {
        private SamplingScheme _samplingScheme;

        [JsonProperty("lossy")]
        public bool Lossy { get; set; } = false;

        [JsonProperty("webp")]
        public bool WebP { get; set; } = false;

        [JsonProperty("auto_orient")]
        public bool AutoOrient { get; set; } = false;

        [JsonProperty("convert")]
        public ConvertImage ConvertImage { get; set; }

        [JsonProperty("resize")]
        public ResizeImage ResizeImage { get; set; }

        [JsonProperty("preserve_meta")]
        public PreserveMeta[] PreserveMeta { get; set; }

        [JsonIgnore]
        public SamplingScheme SamplingScheme
        {
            get { return _samplingScheme; }
            set
            {
                // It's not very elegant, but works.
                _samplingScheme = value;
                SamplingSchemeInternal = ModelHelper.GetSamplingScheme(_samplingScheme);
            }
        }

        [JsonProperty("sampling_scheme")]
        internal string SamplingSchemeInternal { get; set; }

        [JsonProperty("auth")]
        public Authentication Authentication { get; set; } = new Authentication();

        [JsonProperty("dev")]
        public bool Dev { get; set; }
    }
}