using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class OptimizeUploadWaitRequest : OptimizeRequestBase, IOptimizeUploadWaitRequest
    {
        public OptimizeUploadWaitRequest()
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