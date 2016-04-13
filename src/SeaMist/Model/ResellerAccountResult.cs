using Newtonsoft.Json;
using System.Collections.Generic;

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
