using System;

namespace SeaMist.Model
{
    internal static class FactoryOptimizeRequest
    {
        public static OptimizeRequest Create(Uri imageUrl, Uri callbackUrl, IDataStore dataStore)
        {
            switch (dataStore.DataStoreName)
            {
                case "azure_store":
                    return new Azure.OptimizeRequest(imageUrl, callbackUrl, dataStore);
                case "s3_store":
                    return new S3.OptimizeRequest(imageUrl, callbackUrl, dataStore);
                default:
                    throw new InvalidOperationException($"Type {dataStore.DataStoreName} cannot be instantiated");
            }
        }
    }
}