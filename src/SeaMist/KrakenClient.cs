using System;
using System.IO;
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
                throw new ArgumentNullException(nameof(connection));

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

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(string filePath, IOptimizeUploadWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(filePath, optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(string filePath,
            IOptimizeUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken)
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message =
                _connection.ExecuteUpload<OptimizeWaitResult>(new KrakenApiRequest(optimizeWaitRequest, "v1/upload"),
                    file, Path.GetFileName(filePath), cancellationToken);

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

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath, IOptimizeUploadRequest optimizeRequest)
        {
            return Optimize(filePath, optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath,
            IOptimizeUploadRequest optimizeRequest, CancellationToken cancellationToken)
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message = _connection.ExecuteUpload<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                file, filePath, cancellationToken);

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

        // Sets

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(IOptimizeSetWaitRequest optimizeSetWaitRequest)
        {
            return OptimizeWait(optimizeSetWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(IOptimizeSetWaitRequest optimizeSetWaitRequest, CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeSetWaitResults>(new KrakenApiRequest(optimizeSetWaitRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeSetRequest optimizeSetRequest)
        {
            return Optimize(optimizeSetRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeSetRequest optimizeSetRequest, CancellationToken cancellationToken)
        {
            var message = _connection.Execute<OptimizeResult>(new KrakenApiRequest(optimizeSetRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(byte[] image, string filename,
            IOptimizeSetUploadWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(image, filename, optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(byte[] image, string filename,
            IOptimizeSetUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message =
                _connection.ExecuteUpload<OptimizeSetWaitResults>(new KrakenApiRequest(optimizeWaitRequest, "v1/upload"),
                    image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(string filePath, IOptimizeSetUploadWaitRequest optimizeWaitRequest)
        {
            return OptimizeWait(filePath, optimizeWaitRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(string filePath,
            IOptimizeSetUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken)
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message =
                _connection.ExecuteUpload<OptimizeSetWaitResults>(new KrakenApiRequest(optimizeWaitRequest, "v1/upload"),
                    file, Path.GetFileName(filePath), cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeSetUploadRequest optimizeRequest)
        {
            return Optimize(image, filename, optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeSetUploadRequest optimizeRequest, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message = _connection.ExecuteUpload<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath, IOptimizeSetUploadRequest optimizeRequest)
        {
            return Optimize(filePath, optimizeRequest, default(CancellationToken));
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath,
            IOptimizeSetUploadRequest optimizeRequest, CancellationToken cancellationToken)
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message = _connection.ExecuteUpload<OptimizeResult>(new KrakenApiRequest(optimizeRequest, "v1/upload"),
                file, filePath, cancellationToken);

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