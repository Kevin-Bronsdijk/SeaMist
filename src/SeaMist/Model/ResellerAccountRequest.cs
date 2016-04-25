using Newtonsoft.Json;

namespace SeaMist.Model
{
    public class ResellerAccountRequest : IRequest
    {
        public ResellerAccountRequest()
        {
            Authentication = new Authentication();
        }

        [JsonProperty("auth")]
        public Authentication Authentication { get; set; }

        [JsonIgnore]
        public bool Dev { get; set; }
    }
}