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
        }

        [JsonProperty("wait")]
        internal bool Wait { get; set; }

        private void Setup()
        {
            Authentication = new Authentication();
            Wait = true;
            Lossy = false;
            WebP = false;
        }
    }
}