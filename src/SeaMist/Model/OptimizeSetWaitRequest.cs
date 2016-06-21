using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeSetWaitRequest : OptimizeSetRequestBase, IOptimizeSetWaitRequest
    {
        public OptimizeSetWaitRequest(Uri imageUrl)
        {
            ImageUrl = imageUrl;
            Authentication = new Authentication();
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("wait")]
        internal bool Wait { get; set; } = true;
    }
}
