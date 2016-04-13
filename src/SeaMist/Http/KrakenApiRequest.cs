using System.Net.Http;
using SeaMist.Model;

namespace SeaMist.Http
{
    internal class KrakenApiRequest : IApiRequest
    {
        public KrakenApiRequest(IRequest body, string uri)
        {
            Uri = uri;
            Method = HttpMethod.Post;
            Body = body;
        }

        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public IRequest Body { get; set; }
    }
}