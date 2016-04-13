using System;

namespace SeaMist.Model
{
    static class FactoryOptimizeWaitRequest
    {
        public static OptimizeWaitRequest Create(string type, Uri imageUrl, IDataStore dataStore)
        {
            switch (type)
            {
                case "azure_store":
                    return new Azure.OptimizeWaitRequest(imageUrl, dataStore);
                case "s3_store":
                    return new S3.OptimizeWaitRequest(imageUrl, dataStore);
                default:
                    throw new InvalidOperationException(string.Format("Type {0} cannot be instantiated", type));
            }
        }
    }
}
