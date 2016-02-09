using System;
using Newtonsoft.Json;

namespace SeaMist.Model.Azure
{
    public class DataStore : IDataStore
    {
        private string _path = "/";

        public DataStore(string account, string key, string container)
        {
            Account = account;
            Key = key;
            Container = container;
        }

        public DataStore(string account, string key, string container, string path) : 
            this (account, key, container)
        {
            Path = path;
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("container")]
        public string Container { get; set; }

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

        [JsonIgnore]
        public string DataStoreName
        {
            get
            {
                return "azure_store";
            }
        }
    }
}
