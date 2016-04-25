using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeUploadRequest : OptimizeRequestBase, IOptimizeUploadRequest
    {
        public OptimizeUploadRequest()
        {
            Setup();
        }

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
        }
    }
}