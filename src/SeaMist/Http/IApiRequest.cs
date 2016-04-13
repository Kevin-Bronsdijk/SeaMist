using System.Net.Http;
using SeaMist.Model;

namespace SeaMist.Http
{
    internal interface IApiRequest
    {
        string Uri { get; set; }
        IRequest Body { get; set; }
        HttpMethod Method { get; set; }
    }
}