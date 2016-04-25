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
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IApiResponse<ResellerAccountResult>> ResellerAccount()
        {
            return ResellerAccount(default(CancellationToken));
        }

        public Task<IApiResponse<ResellerAccountResult>> ResellerAccount(CancellationToken cancellationToken)
        {
            var userRequest = new ResellerAccountRequest();

            var message = _connection.Execute<ResellerAccountResult>(
                new KrakenApiRequest(userRequest, "v1/subaccounts"), cancellationToken);

            return message;
        }

        public Task<IApiResponse<UserResult>> UserStatus()
        {
            return UserStatus(default(CancellationToken));
        }

        public Task<IApiResponse<UserResult>> UserStatus(CancellationToken cancellationToken)
        {
            var userRequest = new UserRequest();

            var message = _connection.Execute<UserResult>(new KrakenApiRequest(userRequest, "user_status"),
                cancellationToken);

            return message;
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

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(byte[] image, string filename,
            IOptimizeUploadWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(image, filename, optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(byte[] image, string filename,
            IOptimizeUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message =
                _connection.ExecuteUpload<OptimizeWaitResult>(new KrakenApiRequest(optimizeWaitRequest, "v1/upload"),
                    image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeUploadRequest optimizeRequest)
        {
            return Optimize(image, filename, optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeUploadRequest optimizeRequest, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message = _connection.ExecuteUpload<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(IOptimizeWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(IOptimizeWaitRequest optimizeWaitRequest,
            CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeWaitResult>(new KrakenApiRequest(optimizeWaitRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeRequest optimizeRequest)
        {
            return Optimize(optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeRequest optimizeRequest,
            CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        [Obsolete("Method is deprecated, please use OptimizeWait(IOptimizeWaitRequest)")]
        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, IDataStore dataStore)
        {
            return OptimizeWait(imageUri, dataStore, default(CancellationToken));
        }

        [Obsolete("Method is deprecated, please use OptimizeWait(IOptimizeWaitRequest, CancellationToken)")]
        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, IDataStore dataStore,
            CancellationToken cancellationToken)
        {
            var optimizeRequest = FactoryOptimizeWaitRequest.Create(imageUri, dataStore);

            var message = OptimizeWait(optimizeRequest, cancellationToken);

            return message;
        }

        [Obsolete("Method is deprecated, please use OptimizeWait(IOptimizeRequest)")]
        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl, IDataStore dataStore)
        {
            return Optimize(imageUri, callbackUrl, dataStore, default(CancellationToken));
        }

        [Obsolete("Method is deprecated, please use OptimizeWait(IOptimizeRequest, CancellationToken)")]
        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl, IDataStore dataStore,
            CancellationToken cancellationToken)
        {
            var optimizeRequest = FactoryOptimizeRequest.Create(imageUri, callbackUrl, dataStore);

            var message = Optimize(optimizeRequest, cancellationToken);

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