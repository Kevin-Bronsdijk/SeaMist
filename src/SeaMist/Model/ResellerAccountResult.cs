using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class ResellerAccountResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("subaccounts")]
        public List<SubAccount> SubAccounts { get; set; }
    }
}