using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeWaitRequest : OptimizeRequestBase, IOptimizeWaitRequest
    {
        public OptimizeWaitRequest()
        {
            Setup();
        }

        public OptimizeWaitRequest(Uri imageUrl)
        {
            Setup();
            ImageUrl = imageUrl;
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("wait")]
        internal bool Wait { get; set; }

        private void Setup()
        {
            Authentication = new Authentication();
            Wait = true;
        }
    }
}