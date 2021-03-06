using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SeaMist.Model
{
    public class ConvertImage
    {
        public ConvertImage()
        {
            BackgroundColor = "#ffffff";
        }

        public ConvertImage(ImageFormat format)
        {
            Format = format;
            BackgroundColor = "#ffffff";
        }

        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ImageFormat Format { get; set; }

        [JsonProperty("background")]
        public string BackgroundColor { get; set; }
    }
}