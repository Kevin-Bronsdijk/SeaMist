using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaMist.Model.S3
{
    public class DataStore : IDataStore
    {
        public DataStore(string key, string secret, string bucket, string region)
        {
            Key = key;
            Secret = secret;
            Bucket = bucket;
            Region = region;
        }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("acl")]
        public string Acl { get; set; } = "public_read";

        [JsonProperty("headers")]
        public KeyValuePair<string, string> Headers { get; set; } = new KeyValuePair<string, string>();

        [JsonIgnore]
        public string DataStoreName
        {
            get { return "s3_store"; }
        }
    }
}