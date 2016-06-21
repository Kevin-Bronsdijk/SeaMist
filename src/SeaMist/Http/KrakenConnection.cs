using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SeaMist.Model;

namespace SeaMist.Http
{
    public class KrakenConnection : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Uri _krakenApiUrl = new Uri("https://api.kraken.io/");
        private HttpClient _client;
        private JsonMediaTypeFormatter _formatter;

        internal KrakenConnection(string apiKey, string apiSecret, HttpMessageHandler handler, bool sandboxMode)
        {
            _client = new HttpClient(handler) {BaseAddress = _krakenApiUrl};
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            SandboxMode = sandboxMode;

            ConfigureSerialization();

            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public bool SandboxMode { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void ConfigureSerialization()
        {
            _formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
                    Converters = new List<JsonConverter> {new StringEnumConverter {CamelCaseText = true}},
                    NullValueHandling = NullValueHandling.Ignore
                }
            };
        }

        public static KrakenConnection Create(string apiKey, string apiSecret, IWebProxy proxy = null)
        {
            apiKey.ThrowIfNullOrEmpty("apiKey");
            apiSecret.ThrowIfNullOrEmpty("apiSecret");

            var handler = new HttpClientHandler {Proxy = proxy};
            return new KrakenConnection(apiKey, apiSecret, handler, false);
        }

        public static KrakenConnection Create(string apiKey, string apiSecret, bool sandboxMode, IWebProxy proxy = null)
        {
            apiKey.ThrowIfNullOrEmpty("apiKey");
            apiSecret.ThrowIfNullOrEmpty("apiSecret");

            var handler = new HttpClientHandler {Proxy = proxy};
            return new KrakenConnection(apiKey, apiSecret, handler, sandboxMode);
        }

        internal async Task<IApiResponse<TResponse>> Execute<TResponse>(KrakenApiRequest krakenApiRequest,
            CancellationToken cancellationToken)
        {
            krakenApiRequest.Body.Authentication.ApiKey = _apiKey;
            krakenApiRequest.Body.Authentication.ApiSecret = _apiSecret;
            krakenApiRequest.Body.Dev = SandboxMode;
            bool isSet = false;

            if (krakenApiRequest.Body is IOptimizeSetWaitRequest || krakenApiRequest.Body is IOptimizeSetUploadWaitRequest)
            {
                isSet = true;
            }

            using (var requestMessage = new HttpRequestMessage(krakenApiRequest.Method, krakenApiRequest.Uri))
            {
                requestMessage.Content = new ObjectContent(krakenApiRequest.Body.GetType(),
                    krakenApiRequest.Body, _formatter, new MediaTypeHeaderValue("application/json"));


                using (
                    var responseMessage =
                        await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false))
                {

                    return await BuildResponse<TResponse>(responseMessage, cancellationToken, isSet).ConfigureAwait(false);
                }
            }
        }

        internal async Task<IApiResponse<TResponse>> ExecuteUpload<TResponse>(
            KrakenApiRequest krakenApiRequest, byte[] image, string filename, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            krakenApiRequest.Body.Authentication.ApiKey = _apiKey;
            krakenApiRequest.Body.Authentication.ApiSecret = _apiSecret;
            krakenApiRequest.Body.Dev = SandboxMode;
            bool isSet = false;

            if (krakenApiRequest.Body is IOptimizeSetWaitRequest || krakenApiRequest.Body is IOptimizeSetUploadWaitRequest)
            {
                isSet = true;
            }

            using (
                var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var body = new ObjectContent(krakenApiRequest.Body.GetType(),
                    krakenApiRequest.Body, _formatter, new MediaTypeHeaderValue("application/json"));

                content.Add(body);
                content.Add(new StreamContent(new MemoryStream(image)), filename, filename);

                using (var responseMessage = await _client.PostAsync(_krakenApiUrl + krakenApiRequest.Uri, content, cancellationToken))
                {

                    return await BuildResponse<TResponse>(responseMessage, cancellationToken, isSet).ConfigureAwait(false);
                }
            }
        }

        private async Task<IApiResponse<TResponse>> BuildResponse<TResponse>(HttpResponseMessage message,
            CancellationToken cancellationToken, bool isSet)
        {
            var response = new ApiResponse<TResponse>
            {
                StatusCode = message.StatusCode,
                Success = message.IsSuccessStatusCode
            };

            if (message.Content != null)
            {
                if (message.IsSuccessStatusCode)
                {
                    if (isSet)
                    {
                        // Manual parsing required
                        var json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                        // Dont like the link between the Model and Connection, refactor later.
                        var optimizeSetWaitResults = ModelHelper.JsonToSet(json);

                        response.Body = (TResponse)(object)optimizeSetWaitResults;
                    }
                    else
                    {
                        response.Body = await message.Content.ReadAsAsync<TResponse>(new[] { _formatter }, 
                            cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    var errorResponse =
                        await message.Content.ReadAsAsync<ErrorResult>(cancellationToken).ConfigureAwait(false);

                    if (errorResponse != null)
                    {
                        response.Error = errorResponse.Error;
                    }
                }
            }

            return response;
        }

        ~KrakenConnection()
        {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }
    }
}