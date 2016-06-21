using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class SetResizeImage : ResizeImage
    {
        [JsonProperty("id")]
        public string Name { get; set; }

        [JsonProperty("storage_path")]
        public string StoragePath { get; set; }
    }
}
