using System.Net.Http;
using SeaMist.Model;

namespace SeaMist.Http
{
    internal class KrakenApiRequest : IApiRequest
    {
        public KrakenApiRequest(IOptimizeRequest body)
        {
            Uri = "url";
            Method = HttpMethod.Post;
            Body = body;
        }

        public KrakenApiRequest()
        {

        }

        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public IOptimizeRequest Body { get; set; }
    }
}