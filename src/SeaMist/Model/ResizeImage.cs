using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SeaMist.Model
{
    public class ResizeImage
    {
        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("strategy")]
        [JsonConverter(typeof (StringEnumConverter))]
        public Strategy Strategy { get; set; }

        [JsonProperty("background")]
        public string BackgroundColor { get; set; }
    }
}