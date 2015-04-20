using System.Net;

namespace SeaMist.Http
{
    public interface IApiResponse
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
    }
}