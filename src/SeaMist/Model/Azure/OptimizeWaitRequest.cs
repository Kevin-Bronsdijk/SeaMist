using System;
using Newtonsoft.Json;

namespace SeaMist.Model.Azure
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        // Todo: can be altered after removing Obsolete client calls
        public OptimizeWaitRequest(Uri imageUrl, IDataStore dataStore) : base(imageUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, DataStore dataStore) : base(imageUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, string account, string key, string container) : base(imageUrl)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeWaitRequest(Uri imageUrl, string account, string key, string container, string path)
            : base(imageUrl)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        // Todo: DataStore
        [JsonProperty("azure_store")]
        public IDataStore BlobStore { get; set; }
    }
}