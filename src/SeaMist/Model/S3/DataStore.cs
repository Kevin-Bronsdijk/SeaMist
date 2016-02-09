using Newtonsoft.Json;
using System.Collections.Generic;

namespace SeaMist.Model.S3
{
    public class DataStore : IDataStore
    {
        private string _path = string.Empty;
        private string _acl = "public_read";
        private KeyValuePair<string, string> _headers = new KeyValuePair<string, string>(); 

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
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        [JsonProperty("acl")]
        public string Acl
        {
            get
            {
                return _acl;
            }
            set
            {
                _acl = value;
            }
        }

        [JsonProperty("headers")]
        public KeyValuePair<string, string> Headers
        {
            get
            {
                return _headers;
            }
            set
            {
                _headers = value;
            }
        }

        [JsonIgnore]
        public string DataStoreName
        {
            get
            {
                return "s3_store";
            }
        }
    }
}
