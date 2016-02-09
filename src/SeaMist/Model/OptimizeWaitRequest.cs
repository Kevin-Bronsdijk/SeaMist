using System;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeWaitRequest : OptimizeRequestBase
    {
        public OptimizeWaitRequest()
        {
            Setup();
        }

        public OptimizeWaitRequest(Uri imageUrl)
        {
            Setup();
            ImageUrl = imageUrl;
            Authentication = new Authentication();
        }

        [JsonProperty("wait")]
        internal bool Wait { get; set; }

        private void Setup()
        {
            Wait = true;
            Lossy = false;
            WebP = false;
        }
    }
}