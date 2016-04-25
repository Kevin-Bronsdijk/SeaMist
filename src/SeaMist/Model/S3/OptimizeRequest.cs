using System;
using Newtonsoft.Json;

namespace SeaMist.Model.S3
{
    public class OptimizeRequest : Model.OptimizeRequest
    {
        // Todo: can be altered after removing Obsolete client calls
        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, IDataStore dataStore) : base(imageUrl, callbackUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string key, string secret, string bucket, string region)
            : base(imageUrl, callbackUrl)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        // Todo: DataStore
        [JsonProperty("s3_store")]
        public IDataStore S3Store { get; internal set; }
    }
}