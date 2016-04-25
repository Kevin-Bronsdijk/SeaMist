using System;
using Newtonsoft.Json;

namespace SeaMist.Model.S3
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        // Todo: can be altered after removing Obsolete client calls
        public OptimizeWaitRequest(Uri imageUrl, IDataStore dataStore) : base(imageUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, DataStore dataStore) : base(imageUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, string key, string secret, string bucket, string region)
            : base(imageUrl)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        // Todo: DataStore
        [JsonProperty("s3_store")]
        public IDataStore S3Store { get; set; }
    }
}