using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeSetUploadWaitRequest : OptimizeSetRequestBase, IOptimizeSetUploadWaitRequest
    {
        [JsonProperty("wait")]
        internal bool Wait { get; set; } = true;
    }
}