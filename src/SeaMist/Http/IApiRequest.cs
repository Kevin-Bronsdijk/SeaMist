using System.Net.Http;
using SeaMist.Model;

namespace SeaMist.Http
{
    internal interface IApiRequest
    {
        string Uri { get; set; }
        IOptimizeRequest Body { get; set; }
        HttpMethod Method { get; set; }
    }
}