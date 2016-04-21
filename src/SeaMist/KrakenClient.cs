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

            var message = _connection.Execute<ResellerAccountResult>(new KrakenApiRequest(userRequest, "v1/subaccounts"), cancellationToken);

            return message;
        }

        public Task<IApiResponse<UserResult>> UserStatus()
        {
            return UserStatus(default(CancellationToken));
        }

        public Task<IApiResponse<UserResult>> UserStatus(CancellationToken cancellationToken)
        {
            var userRequest = new UserRequest();

            var message = _connection.Execute<UserResult>(new KrakenApiRequest(userRequest, "user_status"), cancellationToken);

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

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(OptimizeWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(OptimizeWaitRequest optimizeWaitRequest, CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeWaitResult>(new KrakenApiRequest(optimizeWaitRequest, "v1/url"), cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, IDataStore dataStore)
        {
            return OptimizeWait(imageUri, dataStore, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, IDataStore dataStore, CancellationToken cancellationToken)
        {
            var optimizeRequest = FactoryOptimizeWaitRequest.Create(dataStore.DataStoreName, imageUri, dataStore);

            var message = OptimizeWait(optimizeRequest, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(byte[] image, string filename)
        {
            return OptimizeWait(image, filename, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(byte[] image, string filename, CancellationToken cancellationToken)
        {
            var optimizeRequest = new OptimizeWaitUploadRequest();

            var message = _connection.ExecuteUpload<OptimizeWaitResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl)
        {
            return Optimize(imageUri, callbackUrl, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl, CancellationToken cancellationToken)
        {
            var optimize = new OptimizeRequest(imageUri, callbackUrl);

            var message = Optimize(optimize, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(OptimizeRequest optimizeRequest)
        {
            return Optimize(optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(OptimizeRequest optimizeRequest, CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename, Uri callbackUrl)
        {
            return Optimize(image, filename, callbackUrl, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename, Uri callbackUrl, CancellationToken cancellationToken)
        {
            var optimizeRequest = new OptimizeUploadRequest(callbackUrl);

            var message = _connection.ExecuteUpload<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

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