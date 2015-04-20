using System;
using System.Threading;
using System.Threading.Tasks;
using SeaMist.Http;
using SeaMist.Model;

namespace SeaMist
{
    public class KrakenClient : IDisposable
    {
        private KrakenConnection _connection;

        public KrakenClient(KrakenConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri)
        {
            return OptimizeWait(imageUri, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, CancellationToken cancellationToken)
        {
            var optimizeRequest = new OptimizeWaitRequest(imageUri);

            var message = OptimizeWait(optimizeRequest, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(OptimizeWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(OptimizeWaitRequest optimizeWaitRequest,
            CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeWaitResult>(new KrakenApiRequest(optimizeWaitRequest), cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl)
        {
            return Optimize(imageUri, callbackUrl, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl,
            CancellationToken cancellationToken)
        {
            var optimize = new OptimizeRequest(imageUri, callbackUrl);

            var message = Optimize(optimize, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(OptimizeRequest optimizeRequest)
        {
            return Optimize(optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(OptimizeRequest optimizeRequest,
            CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeResult>(new KrakenApiRequest(optimizeRequest),
                cancellationToken);

            return message;
        }

        ~KrakenClient()
        {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}