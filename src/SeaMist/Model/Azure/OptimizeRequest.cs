using System;
using Newtonsoft.Json;

namespace SeaMist.Model.Azure
{
    public class OptimizeRequest : Model.OptimizeRequest
    {
        // Todo: can be altered after removing Obsolete client calls
        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, IDataStore dataStore) : base(imageUrl, callbackUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string account, string key, string container)
            : base(imageUrl, callbackUrl)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string account, string key, string container, string path)
            : base(imageUrl, callbackUrl)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        // Todo: DataStore
        [JsonProperty("azure_store")]
        public IDataStore BlobStore { get; internal set; }
    }
}