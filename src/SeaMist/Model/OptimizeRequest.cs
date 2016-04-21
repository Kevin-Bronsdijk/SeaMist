using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeRequest : OptimizeRequestBase
    {
        public OptimizeRequest()
        {
            Setup();
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl)
        {
            Setup();
            ImageUrl = imageUrl;
            CallbackUrl = callbackUrl;
            Authentication = new Authentication();
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }

        private void Setup()
        {
            Lossy = false;
            WebP = false;
        }
    }
}