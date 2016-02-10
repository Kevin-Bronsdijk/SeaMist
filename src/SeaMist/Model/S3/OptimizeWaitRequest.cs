using System;
using Newtonsoft.Json;

namespace SeaMist.Model.S3
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        public OptimizeWaitRequest() : base()
        {
        }

        public OptimizeWaitRequest(Uri imageUrl, IDataStore dataStore) : base(imageUrl)
        {
            S3Store = dataStore;
        }

        [JsonProperty("s3_store")]
        public IDataStore S3Store { get; set; }
    }
}
