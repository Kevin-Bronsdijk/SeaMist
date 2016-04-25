using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeRequest : OptimizeRequestBase, IOptimizeRequest
    {
        public OptimizeRequest(Uri imageUrl, Uri callbackUrl)
        {
            ImageUrl = imageUrl;
            CallbackUrl = callbackUrl;
            Authentication = new Authentication();
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }
    }
}