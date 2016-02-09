using System;
using Newtonsoft.Json;

namespace SeaMist.Model.Azure
{
    public class OptimizeWaitRequest : OptimizeRequestBase
    {
        public OptimizeWaitRequest()
        {
            Setup();
        }

        public OptimizeWaitRequest(Uri imageUrl, IDataStore dataStore)
        {
            Setup();
            ImageUrl = imageUrl;
            BlobStore = dataStore;
            Authentication = new Authentication();
        }

        [JsonProperty("azure_store")]
        public IDataStore BlobStore { get; set; }

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
