using Newtonsoft.Json;
using System;

namespace SeaMist.Model
{
    public class OptimizeUploadRequest : OptimizeRequestBase
    {
        public OptimizeUploadRequest(Uri callbackUrl)
        {
            CallbackUrl = callbackUrl;
            Setup();
        }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }

        private void Setup()
        {
            Authentication = new Authentication();
            Lossy = false;
            WebP = false;
        }
    }
}
