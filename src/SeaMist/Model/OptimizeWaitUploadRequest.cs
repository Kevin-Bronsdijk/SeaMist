using Newtonsoft.Json;
using System;

namespace SeaMist.Model
{
    public class OptimizeWaitUploadRequest : OptimizeRequestBase
    {
        public OptimizeWaitUploadRequest()
        {
            Setup();
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
