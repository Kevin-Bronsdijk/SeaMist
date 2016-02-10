using System;
using Newtonsoft.Json;

namespace SeaMist.Model.Azure
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        public OptimizeWaitRequest() : base()
        {
        }

        public OptimizeWaitRequest(Uri imageUrl, IDataStore dataStore) : base(imageUrl)
        {
            BlobStore = dataStore;
        }

        [JsonProperty("azure_store")]
        public IDataStore BlobStore { get; set; }
    }
}
